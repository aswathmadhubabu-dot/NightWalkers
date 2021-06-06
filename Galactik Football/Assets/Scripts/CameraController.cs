using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject cameraRig;
    public GameObject cam;
    public GameObject car;

    public GameObject ballCameraRig;
    public GameObject target;

    public GameObject ball;

    // Update is called once per frame
    void Update()
    {
        SwitchTarget();
    }

    private void LateUpdate()
    {
        BallCam();
        CarCam();
    }

    private void CarCam()
    {
        if (target == car)
        {
            cam.transform.position = cameraRig.transform.position;
            cam.transform.LookAt(car.transform.position);
        }
    }

    private void BallCam()
    {
        if (target == ball)
        {
            cam.transform.position = ballCameraRig.transform.position;
            cam.transform.LookAt(ball.transform.position);
        }
            
    }

    private void SwitchTarget()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(target == ball)
            {
                target = car;
            }
            else
            {
                target = ball;
            }
        }
    }
}
