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
        if(other.gameObject == ball/* and recentgoal = false*/) { 
            //Goal!
            team.ScoreGoal();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
