using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(VelocityReporter))]
public class PlayersAIScript : MonoBehaviour
{

    public GameObject ball, goalLocation;
    public int ballkickForce = 2;
    public float minimumAlignmentDistance = 4;
    public PlayerState playerState;
    private SkinnedMeshRenderer playerMesh;

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
    void Awake()
    {
        SkinnedMeshRenderer[] mrs = GetComponentsInChildren<SkinnedMeshRenderer>();
        playerMesh = mrs[1];
    }
    // Update is called once per frame
    void Update()
    {
        if (playerState == PlayerState.Idle)
        {
            float distance = (this.transform.position - startPosition).magnitude;
            //Debug.Log("Start Position:  " + startPosition.ToString() + " Ball Position:  " + ball.transform.position.ToString() +  " Player Pos: " + this.transform.position);
            //if (distance > 1)
            //{
            //    Debug.Log("Distance :  " + distance.ToString());
            //    navAgent.SetDestination(startPosition);
            //}
           
            if (navAgent.remainingDistance < 0.1)
            {
                Vector3 lookPos = ball.transform.position - transform.position;
                lookPos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * navAgent.angularSpeed);
            }
        }
        else if (playerState == PlayerState.Attacking)
        {
            Vector3 goalDir = (goalLocation.transform.position - ball.transform.position);
            Vector3 desiredDirection = (ball.transform.position - goalDir.normalized);
            if ((ball.transform.position - transform.position).magnitude > minimumAlignmentDistance)
            {
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
        Vector3 goalDir = (goalLocation.transform.position - ball.transform.position);
        //Debug.Log("BBall Position: " + ball.transform.position.ToString());
        Debug.Log("Velocity: " + velocityReporter.velocity.magnitude.ToString());
        //Debug.Log("Ball Kick Force: " + ballkickForce);
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
    public void InitializeAI(GameObject ball, GameObject goalLocation, string tag, Material mat)
    {
        this.ball = ball;
        this.goalLocation = goalLocation;
        this.gameObject.tag = tag;
        this.playerMesh.material = mat;
    }
}
