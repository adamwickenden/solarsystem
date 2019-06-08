using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Planet : MonoBehaviour {

    //Declare private variables for physics implementation
    private float sunMass;
    private float mass;
    private float force;
    private float G;
    private float planetG;
    private float circularV;
    private Vector3 acceleration;
    private Vector3 currentPosition;
    private Vector3 sunPosition;
    private Vector3 resultant;
    private Rigidbody2D rb2D;
    private Text oscText;
    private List<Sun> sunList;
    private Sprite planetSprite;
    private SpriteRenderer spriteR;
    private Vector3 parentSunPosition;
    private GameObject sun;

    private Vector3 savedVelocity;

    //Declare public variables for physics initialisation
    public Vector2 initialVelocity;
    public static List<Planet> PlanetList;
    public float Damping = 0.1f;
    public static bool planetGravOn = false;
    public bool UserInstantiated = false;
    public float Radius;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();

        if (!UserInstantiated)
        {

        }
    }
    // Use start for initialization
    void Start()
    {
        //Get list of suns and planet G set by player.
        sunList = Sun.SunList;
        planetG = GameObject.Find("Player").GetComponent<Player>().planetG;

        //initialise rb2D it with a velocity kick
        if (!UserInstantiated) {

            int randomInt = Random.Range(3, 21);
            planetSprite = Resources.Load<Sprite>("planet" + randomInt);
            GetComponent<SpriteRenderer>().sprite = planetSprite;
            Radius = Random.Range(0.5f, 2.0f);
            transform.localScale = new Vector3(Radius, Radius, 1);
            float massScale = Mathf.Pow(Radius, 3);
            rb2D.mass = massScale;

            CircularOrbitStart();
        }       
        mass = rb2D.mass;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GravitateSun();
        GravitatePlanets();
        Rotate();
    }

    //Rotate method
    void Rotate()
    {
        resultant = parentSunPosition - transform.position;
        float angle = (Mathf.Atan2(resultant.y, resultant.x) * Mathf.Rad2Deg) - 120f;
        Quaternion qt = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, qt, Time.deltaTime*1000);
    }

    //Gravitate method
    void GravitateSun()
    {
        foreach (Sun sun in sunList)
        {
            sunPosition = sun.transform.position;
            sunMass = sun.GetComponent<Rigidbody2D>().mass;
            G = sun.GetComponent<Sun>().G;

            resultant = sunPosition - transform.position;
            force = (G * sunMass * mass) / Mathf.Pow((resultant.magnitude), 2f);
            acceleration = resultant.normalized * force;
            rb2D.AddForce(acceleration);
        }
    }

    //Handle collision with other objects (sun & boundary)
    private void OnTriggerEnter2D(Collider2D collision)
    {
            //Destruction on collision with sun
            if (collision.gameObject.CompareTag("Sun"))
            {
                Destroy(gameObject);
            }
    }

    void CircularOrbitStart()
    {
        float dold = 0;
        float dnew = 0;
        //Find nearest sun
        foreach (Sun star in sunList)
        {
            if (dold == 0)
            {
                dold = (star.transform.position - transform.position).magnitude;
                dnew = dold;
            }
            else
            {
                dold = dnew;
                dnew = (star.transform.position - transform.position).magnitude;
            }
            if (dnew <= dold) sun = star.gameObject;
        }

        parentSunPosition = sun.transform.position;
        sunMass = sun.GetComponent<Rigidbody2D>().mass;
        G = sun.GetComponent<Sun>().G;
        //Calculate velocity required for circular orbit at given position and extract position x & y
        circularV = Mathf.Sqrt((G * sunMass) / ((parentSunPosition - transform.position).magnitude));

        float x = (parentSunPosition - transform.position).x;
        float y = (parentSunPosition - transform.position).y;
        float vx;
        float vy;

        //claculate perpendicular vector components for any given position, if statements pretect against division/multiplication by 0 and ensure all planets orbit in the same direction.
        if (x == 0) { vx = 1; vy = 0; }
        else if (y == 0) { vx = 0; vy = 1; }
        else if (y > 0) { vx = -1; vy = -(x / y) * vx; }
        else if (y < 0) { vx = 1; vy = -(x / y) * vx; }
        else { vx = 0; vy = 0; }

        //set and normlalise perpendicular vector, then scale it by the required velocity
        Vector2 perpVector = new Vector2(vx, vy);
        Vector2 orbVelocity = perpVector.normalized * circularV;
        rb2D.velocity = orbVelocity;
    }

    //Add and remove planet instances to list of planets
    private void OnEnable()
    {
        if (PlanetList == null) { PlanetList = new List<Planet>(); }
        PlanetList.Add(this);

        rb2D.velocity = savedVelocity;
    }
    private void OnDisable()
    {
        PlanetList.Remove(this);

        savedVelocity = rb2D.velocity;
    }

    //function that loops through list of planets and grivitates towards each one
    void GravitatePlanets()
    {
        if (planetGravOn)
        {
            foreach (Planet planet in PlanetList)
            {
                if (planet != this)
                {                    
                    resultant = planet.transform.position - transform.position;
                    if(resultant.sqrMagnitude <= 500)
                    {
                        force = (planetG * planet.mass * mass) / Mathf.Pow((resultant.magnitude), 2f);
                        //Adds all the resultant vectors to create one final total acceleration due to all planets
                        acceleration = resultant.normalized * force;

                        //Error check for duplicate planets at same position
                        if (float.IsNaN(acceleration.x) || float.IsNaN(acceleration.y) || float.IsNaN(acceleration.z))
                        { acceleration = new Vector3(0, 0, 0); }
                        rb2D.AddForce(acceleration);
                    }                 
                }
            }
        }
        oscText = GameObject.Find("Text").GetComponent<Text>();
        oscText.text = "planet grav on: " + planetGravOn;
    }
}
