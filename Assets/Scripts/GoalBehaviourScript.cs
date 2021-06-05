using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBehaviourScript : MonoBehaviour
{
    public GameObject ball;
    public GameObject ballSpawn;
    public TeamScript team;

    // Start is called before the first frame update
    void Start()
    {
        //ball = new GameObject();

    }

    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == ball) { 
            //Goal!
            ball.transform.position = ballSpawn.transform.position;
            ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            team.score = team.score + 1;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
