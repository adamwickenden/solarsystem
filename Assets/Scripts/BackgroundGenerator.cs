using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundGenerator : MonoBehaviour
{

    private GameObject backgroundSprite;
    private int BGcount = 0;
    private Text oscText;
    private Vector3 placementVector;

    public int xDiv = 25;
    public int yDiv = 25;

    public int xSize = 5;
    public int ySize = 5;

    LayerMask mask;

    // Use this for initialization
    void Awake()
    {
        //On start of the background a 5x5 grid of BGSprites are produced
        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                placementVector = new Vector3(i * xDiv, j * yDiv, 0);
                backgroundSprite = GameObject.Instantiate((GameObject)Resources.Load("BGSprite"), placementVector, Quaternion.identity, GameObject.Find("Background").transform);
                backgroundSprite.name = "BGSprite" + BGcount;
                BGcount++;
            }
        }

        mask = LayerMask.GetMask("Background");
    }

    private void Update()
    {
        //oscText = GameObject.Find("Text").GetComponent<Text>();
        //oscText.text = "Active list: " + BGActiveList.Count + "   Sprite list: " + BGSpriteList.Count;
        ChunkCreation();

    }

    private void ChunkCreation()
    {
        //detect all chunks up
        RaycastHit2D[] hitsup;
        hitsup = Physics2D.RaycastAll(this.transform.position, Vector2.up, 75f, mask);
        //if the number of active chunks above player drops below 2 (thus moving upwards) we need to create (or activate) an new chunck
        if (hitsup.Length < 2)
        {
            //if the chunk next to the empty space has never had a neighbour we need to create one, then we need to iterate through all active chunks in the edge row/column and check for if they do or dont have a neighbour 
            if (!hitsup[0].collider.gameObject.GetComponent<BGSpriteData>().up)
            {
                placementVector = hitsup[0].transform.position + new Vector3(0, yDiv, 0);
                backgroundSprite = GameObject.Instantiate((GameObject)Resources.Load("BGSprite"), placementVector, Quaternion.identity, GameObject.Find("Background").transform);
                backgroundSprite.name = "BGSprite" + BGcount;
                BGcount++;
            }
            //if the chuck has a neighbour, it must be inactive, so set it to active.
            else if (hitsup[0].collider.gameObject.GetComponent<BGSpriteData>().up)
            {
                hitsup[0].collider.gameObject.GetComponent<BGSpriteData>().NeighbourArray[0].SetActive(true);
            }

            //check right by using the currently detected edge piece, checks if it has a neighbour right, then if it does check whether that has an up neighbour, if not create one, if so activate it.
            if (hitsup[0].collider.gameObject.GetComponent<BGSpriteData>().right && hitsup[0].collider.gameObject.GetComponent<BGSpriteData>().NeighbourArray[3].activeInHierarchy)
            {
                var right1 = hitsup[0].collider.gameObject.GetComponent<BGSpriteData>().NeighbourArray[3];
                if (!right1.GetComponent<BGSpriteData>().up)
                {
                    placementVector = right1.transform.position + new Vector3(0, yDiv, 0);
                    backgroundSprite = GameObject.Instantiate((GameObject)Resources.Load("BGSprite"), placementVector, Quaternion.identity, GameObject.Find("Background").transform);
                    backgroundSprite.name = "BGSprite" + BGcount;
                    BGcount++;
                }
                else if (right1.GetComponent<BGSpriteData>().up)
                {
                    right1.GetComponent<BGSpriteData>().NeighbourArray[0].SetActive(true);
                }

                //now we check for a chunck at right + 1 and then follow same creation/activation logic
                if (right1.GetComponent<BGSpriteData>().right && right1.GetComponent<BGSpriteData>().NeighbourArray[3].activeInHierarchy)
                {
                    var right2 = right1.GetComponent<BGSpriteData>().NeighbourArray[3];
                    if (!right2.GetComponent<BGSpriteData>().up)
                    {
                        placementVector = right2.transform.position + new Vector3(0, yDiv, 0);
                        backgroundSprite = GameObject.Instantiate((GameObject)Resources.Load("BGSprite"), placementVector, Quaternion.identity, GameObject.Find("Background").transform);
                        backgroundSprite.name = "BGSprite" + BGcount;
                        BGcount++;
                    }
                    else if (right2.GetComponent<BGSpriteData>().up)
                    {
                        right2.GetComponent<BGSpriteData>().NeighbourArray[0].SetActive(true);
                    }
                }
            }

            //repeat the above but going left.
            if (hitsup[0].collider.gameObject.GetComponent<BGSpriteData>().left && hitsup[0].collider.gameObject.GetComponent<BGSpriteData>().NeighbourArray[2].activeInHierarchy)
            {
                var left1 = hitsup[0].collider.gameObject.GetComponent<BGSpriteData>().NeighbourArray[2];
                if (!left1.GetComponent<BGSpriteData>().up)
                {
                    placementVector = left1.transform.position + new Vector3(0, yDiv, 0);
                    backgroundSprite = GameObject.Instantiate((GameObject)Resources.Load("BGSprite"), placementVector, Quaternion.identity, GameObject.Find("Background").transform);
                    backgroundSprite.name = "BGSprite" + BGcount;
                    BGcount++;
                }
                else if (left1.GetComponent<BGSpriteData>().up)
                {
                    left1.GetComponent<BGSpriteData>().NeighbourArray[0].SetActive(true);
                }

                //now we check for a chunck at left + 1 and then follow same creation/activation logic
                if (left1.GetComponent<BGSpriteData>().left && left1.GetComponent<BGSpriteData>().NeighbourArray[2].activeInHierarchy)
                {
                    var left2 = left1.GetComponent<BGSpriteData>().NeighbourArray[2];
                    if (!left2.GetComponent<BGSpriteData>().up)
                    {
                        placementVector = left2.transform.position + new Vector3(0, yDiv, 0);
                        backgroundSprite = GameObject.Instantiate((GameObject)Resources.Load("BGSprite"), placementVector, Quaternion.identity, GameObject.Find("Background").transform);
                        backgroundSprite.name = "BGSprite" + BGcount;
                        BGcount++;
                    }
                    else if (left2.GetComponent<BGSpriteData>().up)
                    {
                        left2.GetComponent<BGSpriteData>().NeighbourArray[0].SetActive(true);
                    }
                }
            }
        }


        //detect all chunks down
        RaycastHit2D[] hitsDown;
        hitsDown = Physics2D.RaycastAll(this.transform.position, Vector2.down, 75f, mask);
        //if the number of active chunks above player drops below 2 (thus moving upwards) we need to create (or activate) an new chunck
        if (hitsDown.Length < 2)
        {
            //if the chunk next to the empty space has never had a neighbour we need to create one, then we need to iterate through all active chunks in the edge row/column and check for if they do or dont have a neighbour 
            if (!hitsDown[0].collider.gameObject.GetComponent<BGSpriteData>().down)
            {
                placementVector = hitsDown[0].transform.position + new Vector3(0, -yDiv, 0);
                backgroundSprite = GameObject.Instantiate((GameObject)Resources.Load("BGSprite"), placementVector, Quaternion.identity, GameObject.Find("Background").transform);
                backgroundSprite.name = "BGSprite" + BGcount;
                BGcount++;
            }
            //if the chuck has a neighbour, it must be inactive, so set it to active.
            else if (hitsDown[0].collider.gameObject.GetComponent<BGSpriteData>().down)
            {
                hitsDown[0].collider.gameObject.GetComponent<BGSpriteData>().NeighbourArray[1].SetActive(true);
            }

            //check right by using the currently detected edge piece, checks if it has a neighbour right, then if it does check whether that has an up neighbour, if not create one, if so activate it.
            if (hitsDown[0].collider.gameObject.GetComponent<BGSpriteData>().right && hitsDown[0].collider.gameObject.GetComponent<BGSpriteData>().NeighbourArray[3].activeInHierarchy)
            {
                var right1 = hitsDown[0].collider.gameObject.GetComponent<BGSpriteData>().NeighbourArray[3];
                if (!right1.GetComponent<BGSpriteData>().down)
                {
                    placementVector = right1.transform.position + new Vector3(0, -yDiv, 0);
                    backgroundSprite = GameObject.Instantiate((GameObject)Resources.Load("BGSprite"), placementVector, Quaternion.identity, GameObject.Find("Background").transform);
                    backgroundSprite.name = "BGSprite" + BGcount;
                    BGcount++;
                }
                else if (right1.GetComponent<BGSpriteData>().down)
                {
                    right1.GetComponent<BGSpriteData>().NeighbourArray[1].SetActive(true);
                }

                //now we check for a chunck at right + 1 and then follow same creation/activation logic
                if (right1.GetComponent<BGSpriteData>().right && right1.GetComponent<BGSpriteData>().NeighbourArray[3].activeInHierarchy)
                {
                    var right2 = right1.GetComponent<BGSpriteData>().NeighbourArray[3];
                    if (!right2.GetComponent<BGSpriteData>().down)
                    {
                        placementVector = right2.transform.position + new Vector3(0, -yDiv, 0);
                        backgroundSprite = GameObject.Instantiate((GameObject)Resources.Load("BGSprite"), placementVector, Quaternion.identity, GameObject.Find("Background").transform);
                        backgroundSprite.name = "BGSprite" + BGcount;
                        BGcount++;
                    }
                    else if (right2.GetComponent<BGSpriteData>().down)
                    {
                        right2.GetComponent<BGSpriteData>().NeighbourArray[1].SetActive(true);
                    }
                }
            }

            //repeat the above but going left.
            if (hitsDown[0].collider.gameObject.GetComponent<BGSpriteData>().left && hitsDown[0].collider.gameObject.GetComponent<BGSpriteData>().NeighbourArray[2].activeInHierarchy)
            {
                var left1 = hitsDown[0].collider.gameObject.GetComponent<BGSpriteData>().NeighbourArray[2];
                if (!left1.GetComponent<BGSpriteData>().down)
                {
                    placementVector = left1.transform.position + new Vector3(0, -yDiv, 0);
                    backgroundSprite = GameObject.Instantiate((GameObject)Resources.Load("BGSprite"), placementVector, Quaternion.identity, GameObject.Find("Background").transform);
                    backgroundSprite.name = "BGSprite" + BGcount;
                    BGcount++;
                }
                else if (left1.GetComponent<BGSpriteData>().down)
                {
                    left1.GetComponent<BGSpriteData>().NeighbourArray[1].SetActive(true);
                }

                //now we check for a chunck at left + 1 and then follow same creation/activation logic
                if (left1.GetComponent<BGSpriteData>().left && left1.GetComponent<BGSpriteData>().NeighbourArray[2].activeInHierarchy)
                {
                    var left2 = left1.GetComponent<BGSpriteData>().NeighbourArray[2];
                    if (!left2.GetComponent<BGSpriteData>().down)
                    {
                        placementVector = left2.transform.position + new Vector3(0, -yDiv, 0);
                        backgroundSprite = GameObject.Instantiate((GameObject)Resources.Load("BGSprite"), placementVector, Quaternion.identity, GameObject.Find("Background").transform);
                        backgroundSprite.name = "BGSprite" + BGcount;
                        BGcount++;
                    }
                    else if (left2.GetComponent<BGSpriteData>().down)
                    {
                        left2.GetComponent<BGSpriteData>().NeighbourArray[1].SetActive(true);
                    }
                }
            }
        }

        //detect all chunks left
        RaycastHit2D[] hitsLeft;
        hitsLeft = Physics2D.RaycastAll(this.transform.position, Vector2.left, 75f, mask);
        //if the number of active chunks above player drops below 2 (thus moving upwards) we need to create (or activate) an new chunck
        if (hitsLeft.Length < 2)
        {
            //if the chunk next to the empty space has never had a neighbour we need to create one, then we need to iterate through all active chunks in the edge row/column and check for if they do or dont have a neighbour 
            if (!hitsLeft[0].collider.gameObject.GetComponent<BGSpriteData>().left)
            {
                placementVector = hitsLeft[0].transform.position + new Vector3(-xDiv, 0, 0);
                backgroundSprite = GameObject.Instantiate((GameObject)Resources.Load("BGSprite"), placementVector, Quaternion.identity, GameObject.Find("Background").transform);
                backgroundSprite.name = "BGSprite" + BGcount;
                BGcount++;
            }
            //if the chuck has a neighbour, it must be inactive, so set it to active.
            else if (hitsLeft[0].collider.gameObject.GetComponent<BGSpriteData>().left)
            {
                hitsLeft[0].collider.gameObject.GetComponent<BGSpriteData>().NeighbourArray[2].SetActive(true);
            }

            //check right by using the currently detected edge piece, checks if it has a neighbour up, then if it does check whether that has a left neighbour, if not create one, if so activate it.
            if (hitsLeft[0].collider.gameObject.GetComponent<BGSpriteData>().up && hitsLeft[0].collider.gameObject.GetComponent<BGSpriteData>().NeighbourArray[0].activeInHierarchy)
            {
                var up1 = hitsLeft[0].collider.gameObject.GetComponent<BGSpriteData>().NeighbourArray[0];
                if (!up1.GetComponent<BGSpriteData>().left)
                {
                    placementVector = up1.transform.position + new Vector3(-xDiv, 0, 0);
                    backgroundSprite = GameObject.Instantiate((GameObject)Resources.Load("BGSprite"), placementVector, Quaternion.identity, GameObject.Find("Background").transform);
                    backgroundSprite.name = "BGSprite" + BGcount;
                    BGcount++;
                }
                else if (up1.GetComponent<BGSpriteData>().left)
                {
                    up1.GetComponent<BGSpriteData>().NeighbourArray[2].SetActive(true);
                }

                //now we check for a chunck at up + 1 and then follow same creation/activation logic
                if (up1.GetComponent<BGSpriteData>().up && up1.GetComponent<BGSpriteData>().NeighbourArray[0].activeInHierarchy)
                {
                    var up2 = up1.GetComponent<BGSpriteData>().NeighbourArray[0];
                    if (!up2.GetComponent<BGSpriteData>().left)
                    {
                        placementVector = up2.transform.position + new Vector3(-xDiv, 0, 0);
                        backgroundSprite = GameObject.Instantiate((GameObject)Resources.Load("BGSprite"), placementVector, Quaternion.identity, GameObject.Find("Background").transform);
                        backgroundSprite.name = "BGSprite" + BGcount;
                        BGcount++;
                    }
                    else if (up2.GetComponent<BGSpriteData>().left)
                    {
                        up2.GetComponent<BGSpriteData>().NeighbourArray[2].SetActive(true);
                    }
                }
            }

            //repeat the above but going down.
            if (hitsLeft[0].collider.gameObject.GetComponent<BGSpriteData>().down && hitsLeft[0].collider.gameObject.GetComponent<BGSpriteData>().NeighbourArray[1].activeInHierarchy)
            {
                var down1 = hitsLeft[0].collider.gameObject.GetComponent<BGSpriteData>().NeighbourArray[1];
                if (!down1.GetComponent<BGSpriteData>().left)
                {
                    placementVector = down1.transform.position + new Vector3(-xDiv, 0, 0);
                    backgroundSprite = GameObject.Instantiate((GameObject)Resources.Load("BGSprite"), placementVector, Quaternion.identity, GameObject.Find("Background").transform);
                    backgroundSprite.name = "BGSprite" + BGcount;
                    BGcount++;
                }
                else if (down1.GetComponent<BGSpriteData>().left)
                {
                    down1.GetComponent<BGSpriteData>().NeighbourArray[2].SetActive(true);
                }

                //now we check for a chunck at down + 1 and then follow same creation/activation logic
                if (down1.GetComponent<BGSpriteData>().down && down1.GetComponent<BGSpriteData>().NeighbourArray[1].activeInHierarchy)
                {
                    var down2 = down1.GetComponent<BGSpriteData>().NeighbourArray[1];
                    if (!down2.GetComponent<BGSpriteData>().left)
                    {
                        placementVector = down2.transform.position + new Vector3(-xDiv, 0, 0);
                        backgroundSprite = GameObject.Instantiate((GameObject)Resources.Load("BGSprite"), placementVector, Quaternion.identity, GameObject.Find("Background").transform);
                        backgroundSprite.name = "BGSprite" + BGcount;
                        BGcount++;
                    }
                    else if (down2.GetComponent<BGSpriteData>().left)
                    {
                        down2.GetComponent<BGSpriteData>().NeighbourArray[2].SetActive(true);
                    }
                }
            }
        }

        //detect all chunks right
        RaycastHit2D[] hitsRight;
        hitsRight = Physics2D.RaycastAll(this.transform.position, Vector2.right, 75f, mask);
        //if the number of active chunks above player drops below 2 (thus moving upwards) we need to create (or activate) an new chunck
        if (hitsRight.Length < 2)
        {
            //if the chunk next to the empty space has never had a neighbour we need to create one, then we need to iterate through all active chunks in the edge row/column and check for if they do or dont have a neighbour 
            if (!hitsRight[0].collider.gameObject.GetComponent<BGSpriteData>().right)
            {
                placementVector = hitsRight[0].transform.position + new Vector3(xDiv, 0, 0);
                backgroundSprite = GameObject.Instantiate((GameObject)Resources.Load("BGSprite"), placementVector, Quaternion.identity, GameObject.Find("Background").transform);
                backgroundSprite.name = "BGSprite" + BGcount;
                BGcount++;
            }
            //if the chuck has a neighbour, it must be inactive, so set it to active.
            else if (hitsRight[0].collider.gameObject.GetComponent<BGSpriteData>().right)
            {
                hitsRight[0].collider.gameObject.GetComponent<BGSpriteData>().NeighbourArray[3].SetActive(true);
            }

            //check right by using the currently detected edge piece, checks if it has a neighbour up, then if it does check whether that has a left neighbour, if not create one, if so activate it.
            if (hitsRight[0].collider.gameObject.GetComponent<BGSpriteData>().up && hitsRight[0].collider.gameObject.GetComponent<BGSpriteData>().NeighbourArray[0].activeInHierarchy)
            {
                var up1 = hitsRight[0].collider.gameObject.GetComponent<BGSpriteData>().NeighbourArray[0];
                if (!up1.GetComponent<BGSpriteData>().right)
                {
                    placementVector = up1.transform.position + new Vector3(xDiv, 0, 0);
                    backgroundSprite = GameObject.Instantiate((GameObject)Resources.Load("BGSprite"), placementVector, Quaternion.identity, GameObject.Find("Background").transform);
                    backgroundSprite.name = "BGSprite" + BGcount;
                    BGcount++;
                }
                else if (up1.GetComponent<BGSpriteData>().right)
                {
                    up1.GetComponent<BGSpriteData>().NeighbourArray[3].SetActive(true);
                }

                //now we check for a chunck at up + 1 and then follow same creation/activation logic
                if (up1.GetComponent<BGSpriteData>().up && up1.GetComponent<BGSpriteData>().NeighbourArray[0].activeInHierarchy)
                {
                    var up2 = up1.GetComponent<BGSpriteData>().NeighbourArray[0];
                    if (!up2.GetComponent<BGSpriteData>().right)
                    {
                        placementVector = up2.transform.position + new Vector3(xDiv, 0, 0);
                        backgroundSprite = GameObject.Instantiate((GameObject)Resources.Load("BGSprite"), placementVector, Quaternion.identity, GameObject.Find("Background").transform);
                        backgroundSprite.name = "BGSprite" + BGcount;
                        BGcount++;
                    }
                    else if (up2.GetComponent<BGSpriteData>().right)
                    {
                        up2.GetComponent<BGSpriteData>().NeighbourArray[3].SetActive(true);
                    }
                }
            }

            //repeat the above but going down.
            if (hitsRight[0].collider.gameObject.GetComponent<BGSpriteData>().down && hitsRight[0].collider.gameObject.GetComponent<BGSpriteData>().NeighbourArray[1].activeInHierarchy)
            {
                var down1 = hitsRight[0].collider.gameObject.GetComponent<BGSpriteData>().NeighbourArray[1];
                if (!down1.GetComponent<BGSpriteData>().right)
                {
                    placementVector = down1.transform.position + new Vector3(xDiv, 0, 0);
                    backgroundSprite = GameObject.Instantiate((GameObject)Resources.Load("BGSprite"), placementVector, Quaternion.identity, GameObject.Find("Background").transform);
                    backgroundSprite.name = "BGSprite" + BGcount;
                    BGcount++;
                }
                else if (down1.GetComponent<BGSpriteData>().right)
                {
                    down1.GetComponent<BGSpriteData>().NeighbourArray[3].SetActive(true);
                }

                //now we check for a chunck at down + 1 and then follow same creation/activation logic
                if (down1.GetComponent<BGSpriteData>().down && down1.GetComponent<BGSpriteData>().NeighbourArray[1].activeInHierarchy)
                {
                    var down2 = down1.GetComponent<BGSpriteData>().NeighbourArray[1];
                    if (!down2.GetComponent<BGSpriteData>().right)
                    {
                        placementVector = down2.transform.position + new Vector3(xDiv, 0, 0);
                        backgroundSprite = GameObject.Instantiate((GameObject)Resources.Load("BGSprite"), placementVector, Quaternion.identity, GameObject.Find("Background").transform);
                        backgroundSprite.name = "BGSprite" + BGcount;
                        BGcount++;
                    }
                    else if (down2.GetComponent<BGSpriteData>().right)
                    {
                        down2.GetComponent<BGSpriteData>().NeighbourArray[3].SetActive(true);
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BG")
        {
            collision.gameObject.SetActive(false);
        }
    }
}
