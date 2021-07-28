using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

public class AIRobot : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent navmesh;
    private Animator anim;
    private AIState aiState;

    private Rigidbody rb;

    public GameObject target;
    private VelocityReporter vr;
    public float lookAheadTime = 2;

    void Start()
    {
        navmesh = GetComponent<UnityEngine.AI.NavMeshAgent>();  
        anim  = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        aiState = AIState.Wandering;
        vr = target.GetComponent<VelocityReporter>();
        setNextWaypoint();
    }
    void setNextWaypoint(){
        navmesh.SetDestination( this.transform.position +  new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f)));
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 dist = target.transform.position - this.transform.position;

        switch (aiState) {

            case AIState.Wandering:
                if(navmesh.remainingDistance < 1 && !navmesh.pathPending){
                    //Keep wandering around
                    setNextWaypoint();
                }
                if(dist.magnitude < 15 /*AND IS IN CURRENT SIGHT*/){
                    //Im are near the player, lets chase him!
                    aiState = AIState.ChasingPlayer;
                    // Play sound here probably
                    Debug.Log("Detected player!");
                }
            break; 
            case AIState.ChasingPlayer:
                float lookaheadDt = dist.magnitude / vr.maxVelocity.magnitude;
                lookaheadDt = Mathf.Clamp(lookaheadDt, 0.01f, 1.0f);
                Vector3 futurePos = target.transform.position + vr.velocity * lookaheadDt;
                navmesh.SetDestination(futurePos);

                if(navmesh.remainingDistance < 1 && !navmesh.pathPending){
                    aiState = AIState.InRangeOfPlayer;
                }
            break;
            case AIState.InRangeOfPlayer:
            Debug.Log("Got close to player Shooting!");

            break;

        }
        anim.SetFloat("vely", navmesh.velocity.magnitude / navmesh.speed);

    }
    public enum AIState
    {
        ChasingPlayer,
        Wandering,
        InRangeOfPlayer
    };

}
