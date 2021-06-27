using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(VelocityReporter))]
public class EnemyAIScript : MonoBehaviour
{
    public enum EnemyAIState
    {
        Idle,
        Attacking,
        Defending
    }

    public Transform ball, goalLocation;
    private EnemyAIState aiState;
    private NavMeshAgent navAgent;
    private Rigidbody ballRigidBody;
    private VelocityReporter velocityReporter;
 
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("AI Start");
        aiState = EnemyAIState.Attacking;
        navAgent = GetComponent<NavMeshAgent>();
        ballRigidBody = ball.GetComponent<Rigidbody>();
        velocityReporter = GetComponent<VelocityReporter>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 goalDir = (goalLocation.transform.position - ball.position);
        Vector3 desiredDirection = (ball.position - goalDir.normalized * 2);
        navAgent.SetDestination(desiredDirection);
        if (navAgent.remainingDistance < 1.5)
        { 
            if (Vector3.Angle(goalDir, this.transform.forward) < 10)
            {

                    ballRigidBody.AddForce(this.transform.forward * velocityReporter.velocity.magnitude * 10);
            }
            else
            {
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(goalDir), 
                    Time.deltaTime * navAgent.angularSpeed);
            }
        }
    }
}
