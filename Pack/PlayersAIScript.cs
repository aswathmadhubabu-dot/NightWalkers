using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(VelocityReporter))]
public class PlayersAIScript : MonoBehaviour
{
   
    public Transform ball, goalLocation;
    public int ballkickForce = 2;
    public float minimumAlignmentDistance = 4;
    public PlayerState playerState;

    private Vector3 startPosition;
    private VelocityReporter velocityReporter;
    private NavMeshAgent navAgent;

    private Rigidbody ballRigidBody;
    private Collider ballDetectorCollider;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = this.transform.position;
        playerState = PlayerState.Idle;
        navAgent = GetComponent<NavMeshAgent>();
        ballRigidBody = ball.GetComponent<Rigidbody>();
        velocityReporter = GetComponent<VelocityReporter>();
        ballDetectorCollider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerState == PlayerState.Idle)
        {
            float distance = (this.transform.position - startPosition).magnitude;
            if (distance > 1)
            {
                navAgent.SetDestination(startPosition);
            }

            if (navAgent.remainingDistance < 0.1)
            {
                Vector3 lookPos = ball.position - transform.position;
                lookPos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * navAgent.angularSpeed);
            }
        } else if (playerState == PlayerState.Attacking)
        {
            Vector3 goalDir = (goalLocation.transform.position - ball.position);
            Vector3 desiredDirection = (ball.position - goalDir.normalized);
            if ((ball.position - transform.position).magnitude > minimumAlignmentDistance) {
                desiredDirection = desiredDirection * 2;
            }
            Debug.DrawLine(transform.position, desiredDirection, Color.red);

            navAgent.SetDestination(desiredDirection);

            if (navAgent.remainingDistance < 1)
            {
                if (Vector3.Angle(goalDir, this.transform.forward) < 10)
                {
                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(goalDir), 
                        Time.deltaTime * navAgent.angularSpeed);
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            kickBall();
        }
    }

    private void kickBall()
    {
        Vector3 goalDir = (goalLocation.transform.position - ball.position);
        ballRigidBody.AddForce(this.transform.forward * (velocityReporter.velocity.magnitude * ballkickForce), ForceMode.Impulse);
    }

    // Update is called once per frame
    //void Update()
    //{

    //    Vector3 goalDir = (goalLocation.transform.position - ball.position);
    //    Vector3 desiredDirection = (ball.position - goalDir.normalized * 2);
    //    navAgent.SetDestination(desiredDirection);

    //    if (navAgent.remainingDistance < 1) { 

    //        if (Vector3.Angle(goalDir, this.transform.forward) < 10)
    //        {
    //            if (ballReporter.distanceFromGround > 0.01)
    //            {
    //                if (isGrounded)
    //                {
    //                    Debug.Log("In Jump");
    //                }
    //            }
    //            else
    //            {
    //                //ballRigidBody.AddForce(this.transform.forward * velocityReporter.velocity.magnitude * 10);
    //            }
    //        }
    //        else
    //        {
    //            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(goalDir), 
    //                Time.deltaTime * navAgent.angularSpeed);
    //        }
    //    }
    //}
}
