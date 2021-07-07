using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(VelocityReporter))]
public class AIGoalie : MonoBehaviour
{

    public GameObject ball;
    public Vector3 goalA;
    public Vector3 goalB;
    public int ballkickForce = 4;
    public float minimumAlignmentDistance = 4;
    public enum GoalieState
    {
        Idle,
        Defending
    }

    public GoalieState goalieState;

    private Vector3 startPosition;
    private VelocityReporter velocityReporter;
    private NavMeshAgent navAgent;

    private Rigidbody ballRigidBody;
    private Collider ballDetectorCollider;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = this.transform.position;
        goalieState = GoalieState.Idle;
        navAgent = GetComponent<NavMeshAgent>();
        ballRigidBody = ball.GetComponent<Rigidbody>();
        velocityReporter = GetComponent<VelocityReporter>();
        ballDetectorCollider = GetComponent<SphereCollider>();
    }


    // Update is called once per frame
    void Update()
    {

        float time = Mathf.PingPong(Time.time * 2.5f, 1);
        this.transform.position = Vector3.Lerp(goalA, goalB, time);
    }
}
