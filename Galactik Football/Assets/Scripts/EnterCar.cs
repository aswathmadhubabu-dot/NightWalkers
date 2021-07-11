using UnityEngine;
using UnityEngine.Serialization;

public class EnterCar : MonoBehaviour
{
    public GameObject carCamera;
    public GameObject player;
    public GameObject exitTrigger;
    public GameObject car;
    public int triggerCheck;

    private void OnTriggerEnter(Collider other) => triggerCheck = 1;

    private void OnTriggerExit(Collider other) => triggerCheck = 0;

    void Update()
    {
        if (triggerCheck == 1 && Input.GetButtonDown("Action"))
        {
            carCamera.SetActive(true);
            player.SetActive(false);
            ((Behaviour) car.GetComponent("CarController")).enabled = true;
            ((Behaviour) car.GetComponent("CarUserControl")).enabled = true;
            exitTrigger.SetActive(true);
        }
    }
}