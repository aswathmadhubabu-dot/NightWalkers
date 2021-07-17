using System.Collections;
using UnityEngine;


[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class ShooterRobotScript : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent navmesh;
    private Animator anim;
    private AIState aiState;

    private Rigidbody rb;

    public GameObject target;
    private VelocityReporter vr;
    public float lookAheadTime = 2;

    private Vector3 worldDeltaPosition;
    private Vector2 groundDeltaPosition;
    private Vector2 velocity = Vector2.zero;
    private float spotArrivalTime;

    public float timeInSpots = 15;
    private float turnVel;
    private float forwardVel;

    public Transform raycastOrigin;
    private Ray ray;
    private RaycastHit hitInfo;

    void Start()
    {
        navmesh = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        vr = target.GetComponent<VelocityReporter>();
        navmesh.updatePosition = false;
        setNextWaypoint();
        //anim.applyRootMotion = true;
    }

    void setNextWaypoint()
    {
        Debug.Log("Bored here, moving to new point");
        //navmesh.stoppingDistance = 1;
        navmesh.SetDestination(this.transform.position +
                               new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f)));
        //TODO CHECK IF DESTINATION IS IN FIELD. OTHERWISE GENERATE A NEW ONE.
        aiState = AIState.Wandering;
    }

    void OnAnimatorMove()
    {
        this.transform.position = navmesh.nextPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dist = target.transform.position - this.transform.position;
        bool stopping;
        switch (aiState)
        {
            case AIState.OnASpot:
                if (Time.time - spotArrivalTime > timeInSpots)
                {
                    setNextWaypoint();
                }

                CheckIfRobotSeesPlayer(dist);
                break;
            case AIState.Wandering:
                stopping = false;
                if (navmesh.remainingDistance < 3 && !navmesh.pathPending)
                {
                    stopping = true;
                    //Keep wandering around
                }

                MoveRootMotionRobot(stopping);
                if (navmesh.remainingDistance < 1 && !navmesh.pathPending)
                {
                    StopRobot();
                    StayOnSpot();
                }

                CheckIfRobotSeesPlayer(dist);
                break;
            case AIState.ChasingPlayer:

                CalculateDistanceAndChasePlayer(dist);
                stopping = false;
                if (navmesh.remainingDistance < 10)
                {
                    stopping = true;
                }

                MoveRootMotionRobot(stopping);

                if (isPlayerInSight())
                {
                    if (navmesh.remainingDistance < 10 && !navmesh.pathPending)
                    {
                        StopRobot();
                        aiState = AIState.InRangeOfPlayer;
                    }
                }
                else
                {
                    //Lost Player from Sight.
                    Debug.Log("Lost Player, staying here");
                    StopRobot();
                }

                break;
            case AIState.InRangeOfPlayer:
                anim.SetBool("aiming", true);
                MoveRootMotionRobot(true);

                Debug.Log("Got close to player Shooting! " + navmesh.remainingDistance.ToString());
                StartCoroutine(WaitAndFire());

                break;
        }
    }

    void CalculateDistanceAndChasePlayer(Vector3 dist)
    {
        navmesh.stoppingDistance = 10;
        float lookaheadDt = dist.magnitude / vr.maxVelocity.magnitude;
        lookaheadDt = Mathf.Clamp(lookaheadDt, 0.01f, 1.0f);
        Vector3 futurePos = target.transform.position + vr.velocity * lookaheadDt;
        navmesh.SetDestination(futurePos);
    }

    IEnumerator WaitAndFire()
    {
        //Print the time of when the function is first called.
        //Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(2);
        Fire();
        //After we have waited 5 seconds print the time again.
        //Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

    void Fire()
    {
        ray.origin = raycastOrigin.position;
        ray.direction = raycastOrigin.forward;

        if (Physics.Raycast(ray, out hitInfo))
        {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1.0f);
        }
    }

    void CheckIfRobotSeesPlayer(Vector3 dist)
    {
        if (dist.magnitude < 35 && isPlayerInSight())
        {
            //Can see the player, lets chase him!
            aiState = AIState.ChasingPlayer;
            // Play sound here probably
            Debug.Log("Detected player!");
        }
    }

    void StayOnSpot()
    {
        aiState = AIState.OnASpot;
        spotArrivalTime = Time.time;
    }

    void SetWandering()
    {
        StopRobot();
        aiState = AIState.Wandering;
    }

    void StopRobot()
    {
        navmesh.isStopped = true;
        navmesh.ResetPath();
    }

    void MoveRootMotionRobot(bool stopping)
    {
        worldDeltaPosition = navmesh.nextPosition - transform.position;
        groundDeltaPosition.x = Vector3.Dot(transform.right, worldDeltaPosition);
        groundDeltaPosition.y = Vector3.Dot(transform.forward, worldDeltaPosition);
        velocity = (Time.deltaTime > 1e-5f) ? groundDeltaPosition / Time.deltaTime : Vector2.zero;
        bool moving = velocity.magnitude > 0.025f && navmesh.remainingDistance > navmesh.radius;


        if (stopping)
        {
            forwardVel = Mathf.Lerp(forwardVel, 0, Time.deltaTime * 10);
            turnVel = Mathf.Lerp(turnVel, 0, Time.deltaTime * 10);
            if (forwardVel < 0.05)
            {
                forwardVel = 0;
                Debug.Log("SET0");
            }

            if (turnVel < 0.05)
            {
                turnVel = 0;
            }
        }
        else
        {
            forwardVel = velocity.y;
            turnVel = velocity.x;
        }

        Debug.Log(forwardVel);

        anim.SetBool("moving", moving);
        anim.SetFloat("vely", forwardVel);
        anim.SetFloat("velx", turnVel);
    }

    bool isPlayerInSight()
    {
        Vector3 targetDir = target.transform.position - transform.position;
        float angleToPlayer = (Vector3.Angle(targetDir, transform.forward));

        if (angleToPlayer >= -70 && angleToPlayer <= 70)
        {
            return true;
        }
        else
        {
            return false;
        } // 180° FOV
        //Debug.Log("Player in sight!");
    }

    private enum AIState
    {
        ChasingPlayer,
        Wandering,
        OnASpot,
        InRangeOfPlayer
    };
}