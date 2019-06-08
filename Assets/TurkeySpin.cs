using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurkeySpin : MonoBehaviour {

    private Transform transform;
    private Vector3 rotation;
    private GameObject gameObject;
    private GameObject player;
    private ParticleSystem particles;

    private int deathTimer, deathLimit = 2000;

    private float rotationSpeed = 0.02f;
    private bool clockwise = false;

	// Use this for initialization
	void Start () {
        transform = GetComponent<Transform>();
        gameObject = GameObject.Find("Turkey");
        player = GameObject.Find("Player");
        particles = GetComponent<ParticleSystem>();
        particles.Pause();

        rotation = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {

        deathTimer++;

        if (rotation.z > 5 || rotation.z < -5)
        {
            particles.Play();
            clockwise = !clockwise;
        }
        if (clockwise) rotation.z += rotationSpeed; else rotation.z -= rotationSpeed;

        transform.Rotate(rotation);

        if (deathTimer > deathLimit)
        {
            particles.Play();
        }

        if (deathTimer > deathLimit + 200)
        {
            Destroy(gameObject);
        }

        float interpolation = 0.2f * Time.deltaTime;
        //Lerp interpolates from one number to another given by an interpolation value - jittery when moving quickly

        Vector3 position = this.transform.position;
        position.x = Mathf.Lerp(this.transform.position.x, player.transform.position.x, interpolation);
        position.y = Mathf.Lerp(this.transform.position.y, player.transform.position.y, interpolation);

        this.transform.position = position;
    }
}
