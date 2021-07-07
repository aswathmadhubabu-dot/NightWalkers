using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class VelocityReporter : MonoBehaviour
{
    private Vector3 prevPos; public Vector3 rawVelocity
    {
        get;
        private set;
    }

    public Vector3 velocity
    {
        get;
        private set;
    }

    public float smoothingTimeFactor = 0.5f;
    private Vector3 smoothingParamVel = new Vector3(0.0f, 0.0f, 0.0f);

    // Use this for initialization 
    void Start()
    {
        prevPos = this.transform.position;
    }


    // Update is called once per frame 
    void Update()
    {
        rawVelocity = (this.transform.position - prevPos) / Time.deltaTime;
        Debug.Log("Velocity in VEloc: " + velocity.ToString());
        Debug.Log("Raw Velocity: " + rawVelocity.ToString());
        Debug.Log("Smoothing ParamVel: " + smoothingParamVel.ToString());
        Debug.Log("smooth time: " + smoothingTimeFactor.ToString());

        velocity = Vector3.SmoothDamp(velocity, rawVelocity, ref smoothingParamVel, smoothingTimeFactor);
        prevPos = this.transform.position;
    }
}