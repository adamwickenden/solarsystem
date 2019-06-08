using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    //Private variables
    private Vector3 resultant;
    private Rigidbody2D rb2D;
    private Text oscText;
    private GameObject playerPlanet;
    private float startTime;
    private float fireTime;
    private bool firstPress;

    //public variables
    public float speed;
    public float rotationSpeed = 100f;
    public float attackStrength = 100f;
    public float planetG;
    public float planetDistance = 1.8f;
    public float createTime = 2f;
    public float maxFireHold = 4f;
    public float planetFirePower = 50f;


    // Use this for initialization
    void Start () {
        rb2D = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {        
        MovePlayer();
        setPlanetGrav();
        planetGun();
    }

    /*
    //Rotate front of ship toward mouse at all times  (Not using now, keeping just for the sake of keeping)
    void Rotate()
    {

        resultant = camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = (Mathf.Atan2(resultant.y, resultant.x) * Mathf.Rad2Deg) - 90f;
        Quaternion qt = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, qt, Time.deltaTime * rotationSpeed);
    }
    */

    //Move player taking input from wasd
    void MovePlayer()
    {
        rb2D.velocity = Vector3.zero;
        rb2D.angularVelocity = 0f;

        float rotate = Input.GetAxis("Horizontal") * rotationSpeed * -1;
        float translate = Input.GetAxis("Vertical") * speed;

        Vector3 translation = new Vector3(0, translate, 0);
        Vector3 rotation = new Vector3(0,0, rotate);

        transform.Translate(translation);
        transform.Rotate(rotation);

    }

    //Basic attack which just blasts a planet in a specific direction
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Planet"))
        {
            if (Input.GetKeyDown(KeyCode.Space)) { collision.GetComponent<Rigidbody2D>().AddForce(transform.up * attackStrength, ForceMode2D.Impulse); }            
        }
    }

    //Ability to turn planet gravity on and off
    private void setPlanetGrav()
    {
        //must loop through list of planets and set on for each one as being set on by Player and not in the Planet script.
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!Planet.planetGravOn) { Planet.planetGravOn = true; }
            else { Planet.planetGravOn = false; }
        }
    }

    //Attack which generates a planet and fires it along the vector3.forward direction of the sprite, amount of time held down for determines velocity up to a max of ... 
    //(possibly doing hold for x time to create which sets size/mass, then y time for velocity)  
    private void planetGun()
    {
        //on key down set a time reference and the first press bool to true
        if (Input.GetKeyDown(KeyCode.E))
        {
            startTime = Time.time;
            firstPress = true;
        }
        if (Input.GetKey(KeyCode.E))
        {
            //after 2 seconds this returns true
            if (startTime + createTime <= Time.time)
            {
                //so if its the "first press" i.e. a planet hasn't been made create one  that is UserInstantiated
                if (firstPress)
                {
                    playerPlanet = GameObject.Instantiate((GameObject)Resources.Load("Planet"), transform.position + (planetDistance * transform.up), Quaternion.identity, GameObject.Find("Foreground").transform);
                    playerPlanet.GetComponent<Planet>().UserInstantiated = true;
                    playerPlanet.SetActive(true);

                    //set first press to false and assign new tiime reference for fire strength percentage
                    firstPress = false;
                    fireTime = Time.time;
                }
                //if a planet has been created and key is still held move it with the ship. Checking for if the planet still exists to avoid nullreference error if flown into sun prior to firing
                else{ if (playerPlanet) { playerPlanet.transform.position = transform.position + (planetDistance * transform.up); } }                
            }
        }
        //When key is released and a planet has been created, fire it off at a given percentage of a max value dependant on hold time. If hold time is greater than boundary value fire at 100%. object existance checks in place for null reference avoidance
        if (Input.GetKeyUp(KeyCode.E) && !firstPress)
        {
            float fireRatio = (Time.time - fireTime) / maxFireHold;
            if (fireRatio <= 1f) {
                if (playerPlanet) { playerPlanet.GetComponent<Rigidbody2D>().AddForce(transform.up * (planetFirePower * fireRatio), ForceMode2D.Impulse); }
            }
            else { if (playerPlanet) { playerPlanet.GetComponent<Rigidbody2D>().AddForce(transform.up * planetFirePower, ForceMode2D.Impulse); } }
        }
    }
}
