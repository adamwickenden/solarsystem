using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Sun : MonoBehaviour {

    //Declare private variables for animation
    private float angle;
    private float xScaleOriginal;
    private float xScaleAdjusted;
    private float yScaleOriginal;
    private float yScaleAdjusted;

    //Declare public variables for physics
    public float G;
    public static List<Sun> SunList;

    // Use this for initialization
    void Start () {
        //initialise angle to zero
        angle = transform.localRotation.z;
        xScaleOriginal = transform.localScale.x;
        yScaleOriginal = transform.localScale.y;
    }

    //Add and remove sun instances to list of suns (cba to rename to stars)
    private void OnEnable()
    {
        if (SunList == null) { SunList = new List<Sun>(); }
        SunList.Add(this);
    }
    private void OnDisable()
    {
        SunList.Remove(this);
    }

    // Update is called once per frame
    void FixedUpdate () {
        Rotate();
        Pulse();
    }

    //Method to make star rotate 1 degree per frame
    void Rotate()
    {
        if(angle >= 360) { angle = 0; }
        else { angle++; }
        Vector3 angleVect = new Vector3(0, 0, angle);
        transform.eulerAngles = angleVect;   
    }

    //Method to make star randomly pulsate
    void Pulse()
    {       
        //Create number that oscillates as a sine wave (-1 <-> 1), use this to make the X and Y scales oscillate too
        float scaleShift = (float)Math.Sin(angle * (Math.PI/180f));
        xScaleAdjusted = xScaleOriginal + 0.1f * scaleShift;
        yScaleAdjusted = yScaleOriginal - 0.1f * scaleShift;
        Vector3 scaleVect = new Vector3(xScaleAdjusted, yScaleAdjusted, 0f);
        transform.localScale = scaleVect;  
    }
}
