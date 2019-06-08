using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSpriteData : MonoBehaviour
{

    public bool up;
    public bool down;
    public bool left;
    public bool right;

    // [0] = up, [1] = down, [2] = left, [3] = right
    public GameObject[] NeighbourArray = new GameObject[4];

    private bool hasSolarSystem;
    public List<GameObject> SolarSystemList;
    public float solarSpawnChance = 0.1f;
    public int maxNoPlanets = 6;
    public float solarRadius = 20f;


    // Use this for initialization
    private void Start()
    {
        CheckNeighbours();
        SolarSystemSpawn();
    }

    private void CheckNeighbours()
    {
        //set layermask that makes raycast only detect background colliders
        LayerMask mask = LayerMask.GetMask("Background");

        //check for object up
        RaycastHit2D neighbour = Physics2D.Raycast(this.transform.position, Vector2.up, 25f, mask);
        if (neighbour)
        {
            up = true;
            NeighbourArray[0] = neighbour.collider.gameObject;
            //object that is above the new BG object won't know it's there so we need to set its neighbour list to contain the new object
            if (!neighbour.collider.gameObject.GetComponent<BGSpriteData>().down)
            {
                neighbour.collider.gameObject.GetComponent<BGSpriteData>().down = true;
                neighbour.collider.gameObject.GetComponent<BGSpriteData>().NeighbourArray[1] = this.gameObject;
            }
        }

        //check for object down
        neighbour = Physics2D.Raycast(this.transform.position, Vector2.down, 25f, mask);
        if (neighbour)
        {
            down = true;
            NeighbourArray[1] = neighbour.collider.gameObject;

            if (!neighbour.collider.gameObject.GetComponent<BGSpriteData>().up)
            {
                neighbour.collider.gameObject.GetComponent<BGSpriteData>().up = true;
                neighbour.collider.gameObject.GetComponent<BGSpriteData>().NeighbourArray[0] = this.gameObject;
            }
        }

        //check for object left
        neighbour = Physics2D.Raycast(this.transform.position, Vector2.left, 25f, mask);
        if (neighbour)
        {
            left = true;
            NeighbourArray[2] = neighbour.collider.gameObject;

            if (!neighbour.collider.gameObject.GetComponent<BGSpriteData>().right)
            {
                neighbour.collider.gameObject.GetComponent<BGSpriteData>().right = true;
                neighbour.collider.gameObject.GetComponent<BGSpriteData>().NeighbourArray[3] = this.gameObject;
            }
        }

        //check for object right
        neighbour = Physics2D.Raycast(this.transform.position, Vector2.right, 25f, mask);
        if (neighbour)
        {
            right = true;
            NeighbourArray[3] = neighbour.collider.gameObject;

            if (!neighbour.collider.gameObject.GetComponent<BGSpriteData>().left)
            {
                neighbour.collider.gameObject.GetComponent<BGSpriteData>().left = true;
                neighbour.collider.gameObject.GetComponent<BGSpriteData>().NeighbourArray[2] = this.gameObject;
            }
        }
    }

    //Function to randomly spawn a solar system on initialisation 
    private void SolarSystemSpawn()
    {
        //If random number is < spawn chance a sun will be created at the centre point of the BGChunk
        if (Random.value < solarSpawnChance)
        {
            hasSolarSystem = true;
            GameObject SolarSystem = new GameObject();
            SolarSystem.name = this.name + " Solar System";
            SolarSystem.transform.parent = this.transform;
            GameObject sun = GameObject.Instantiate((GameObject)Resources.Load("Sun"), transform.position, Quaternion.identity, SolarSystem.transform);
            sun.name = "Sun";
            SolarSystemList.Add(sun);

            int noPlanets = Random.Range(1, maxNoPlanets);

            for (int i = 0; i <= noPlanets; i++)
            {
                Vector3 planetPosition = Random.insideUnitCircle * solarRadius;
                GameObject planet = GameObject.Instantiate((GameObject)Resources.Load("Planet"), transform.position + planetPosition, Quaternion.identity, SolarSystem.transform);
                planet.name = "Planet " + i;
                SolarSystemList.Add(planet);
            }
        }
    }


    //Function to enable solar system if BGChunk already enabled and has a solar system
    private void OnEnable()
    {
        if (hasSolarSystem)
        {
            //reapply velocity
        }
    }

    //Function to disable solar system if it has one
    private void OnDisable()
    {
        if (hasSolarSystem)
        {
            //remember velocity
        }
    }
}
