using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {

    private GameObject playerToFollow;
    public float speed = 1.0f;

    void Start()
    {
        playerToFollow = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update () {
        MoveCamera();
	}

    private void MoveCamera()
    {
        float interpolation = speed * Time.deltaTime;
        //Lerp interpolates from one number to another given by an interpolation value - jittery when moving quickly

        Vector3 position = this.transform.position;
        position.x = Mathf.Lerp(this.transform.position.x, playerToFollow.transform.position.x, interpolation);
        position.y = Mathf.Lerp(this.transform.position.y, playerToFollow.transform.position.y, interpolation);

        this.transform.position = position;
    }
}
