using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCar : MonoBehaviour
{
    public GameObject _carCam;
    public GameObject _player;
    public GameObject _exitTrigger;
    public GameObject _car;
    public GameObject _exitPlace;

    void Update()
    {
        if (Input.GetButtonDown("Action"))
        {
            _player.SetActive(true);
            _player.transform.position = _exitPlace.transform.position;
            _carCam.SetActive(false);
            ((Behaviour) _car.GetComponent("CarController")).enabled = false;
            ((Behaviour) _car.GetComponent("CarUserControl")).enabled = false;
            _exitTrigger.SetActive(false);
        }
    }
}