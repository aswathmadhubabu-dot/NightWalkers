using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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
    public Ray ray;
    public RaycastHit hitInfo;
    public TrailRenderer tracerEffect;
    public bool firing;
    public Transform visionOrigin;

    public ParticleSystem muzzleFlash;
    private float lastSeenPlayer = -9999f;

    private
    //Distance to target
    Vector3 dist;
    Vector3 finalPosition;
    void Start()
    {
        navmesh = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        vr = target.GetComponent<VelocityReporter>();
        navmesh.updatePosition = false;
        setNextWaypoint();
        firing = false;
        //anim.applyRootMotion = true;
        
    }

    void setNextWaypoint()
    {
        Debug.Log("Bored here, moving to new point");
        navmesh.stoppingDistance = 1;
        NavMeshHit hit;
        float walkRadius = 10f;
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        NavMesh.SamplePosition(this.transform.position + randomDirection, out hit, walkRadius, 1);
        finalPosition = hit.position;
        navmesh.SetDestination(finalPosition);
        //TODO CHECK IF DESTINATION IS IN FIELD. OTHERWISE GENERATE A NEW ONE.
        aiState = AIState.Wandering;
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawCube(finalPosition,new Vector3(0.3f, 0.3f, 0.3f));
    }

    void OnAnimatorMove()
    {
        this.transform.position = navmesh.nextPosition;
    }

    // Update is called once per frame
    void Update()
    {
        this.dist = target.transform.position - this.transform.position;
        bool stopping;
        switch (aiState) {
            case AIState.OnASpot:
                Debug.Log("Stayin on spot");
                if(Time.time - spotArrivalTime  > timeInSpots){
                    setNextWaypoint();
                }
                MoveRootMotionRobot(true);
                CheckIfRobotSeesPlayer(dist);
                break;
            case AIState.Wandering:
                Debug.Log("wandering ");

                stopping = false;
                if(navmesh.remainingDistance < 1){
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
                
                if(isPlayerInSight()){
                    //Check if should rotate
                    if(navmesh.remainingDistance < 10 && !navmesh.pathPending){
                        StopRobot();
                        aiState = AIState.InRangeOfPlayer;
                    }
                }
                else
                {
                    //Lost Player from Sight.
                    Debug.Log("Lost Player, staying here");
                    StopRobot();
                    StayOnSpot();
                }

                break;
            case AIState.InRangeOfPlayer:
                anim.SetBool("aiming", true);
                MoveRootMotionRobot(true);
                Debug.Log("Close to player!");

                if(aimingAtTarget()){
                    if(!firing){
                        Debug.Log("Aiming, Shooting! " + navmesh.remainingDistance.ToString());
                        StartCoroutine(WaitAndFire());
                    }
                } else{
                    RotateToTarget();
                }
            break;
        }
    }


    bool aimingAtTarget(){
        Vector3 targetDir = target.transform.position - transform.position;
        float angleToPlayer = (Vector3.Angle(targetDir, transform.forward));
        return (angleToPlayer < 10);
    }

    void CalculateDistanceAndChasePlayer(Vector3 dist){
        navmesh.stoppingDistance = 10;
        float lookaheadDt = dist.magnitude / vr.maxVelocity.magnitude;
        lookaheadDt = Mathf.Clamp(lookaheadDt, 0.01f, 1.0f);
        Vector3 futurePos = target.transform.position + vr.velocity * lookaheadDt;
        navmesh.SetDestination(futurePos);
    }

    IEnumerator WaitAndFire()
    {
        firing = true;
        //Print the time of when the function is first called.
        //Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(0.3f);
        Aim();
        yield return new WaitForSeconds(1);
        Fire();
        //After we have waited 5 seconds print the time again.
        //Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        yield return new WaitForSeconds(1);
        firing = false;
        aiState = AIState.ChasingPlayer;
    }
    void Aim(){
        ray.origin = raycastOrigin.position;
        //ray.direction = raycastOrigin.forward;

        var tracer = Instantiate(tracerEffect, ray.origin, Quaternion.identity);
        tracer.AddPosition(ray.origin);
        ray.direction = target.transform.position + new Vector3(0f,1.5f,0f) - ray.origin;
        if(Physics.Raycast(ray, out hitInfo)) {
            //Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1.0f);
            tracer.transform.position = hitInfo.point;
        }
    }
    void Fire(){
        muzzleFlash.Emit(200);
        if(Physics.Raycast(ray, out hitInfo)) {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1.0f);
            if(hitInfo.collider.gameObject == target){
                Debug.Log("Hit Player!");
                //drop ball
            } else {
                Debug.Log("Missed!");
            }

        } else {
            Debug.Log("Missed!");
        }
    }

    void CheckIfRobotSeesPlayer(Vector3 dist)
    {
        if (dist.magnitude < 35 && isPlayerInSight())
        {
            //Can see the player, lets chase him!
            aiState = AIState.ChasingPlayer;
            // Play sound here probably
            Debug.Log("Player is on sight");
        } else{
            if(aiState == AIState.InRangeOfPlayer){
                StayOnSpot();
            }
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
    void RotateToTarget(){
        Vector3 targetDir = target.transform.position - transform.position;
        float angleToPlayer = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);

        //Debug.Log("ROTATING " + angleToPlayer);
        this.transform.Rotate(new Vector3(0, -angleToPlayer * Time.deltaTime, 0));
        anim.SetBool("moving",true);
        anim.SetFloat("velx", 0.3f);
    }
    void MoveRootMotionRobot(bool stopping){
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
                //Debug.Log("SET0");

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
        
        //TODO play footsteps
        anim.SetBool("moving", moving);
        anim.SetFloat("vely", forwardVel);
        anim.SetFloat("velx", turnVel);
    }

    bool isPlayerInSight()
    {
        Vector3 targetDir = target.transform.position - transform.position;
        float angleToPlayer = (Vector3.SignedAngle(targetDir, transform.forward, Vector3.up));
        bool seenPlayer = false;
        if (angleToPlayer >= -70 && angleToPlayer <= 70){
            //If angles are correct, Raycast to see if it can see the char.
            //Debug.Log("Watching around: " + targetDir);
            
            for(float i = -358; i <= -290; i += 2 ){
                seenPlayer = ViewForAngle(i);
            }
            //Ugly hack to prevent GimbalLock
            for(float i = -70; i <= -1; i += 2 ){
                seenPlayer = ViewForAngle(i) || seenPlayer;
            }
        }
        
        if(seenPlayer) {
            lastSeenPlayer = Time.time;
        } 
        //IF seen player in the last 5 seconds, then chase
        float currentLastSeen = lastSeenPlayer + 5;
        if(currentLastSeen > Time.time){
            return true;
        }
        return false;
    }
    bool ViewForAngle(float i){
        RaycastHit hit;
        Vector3 headForward = visionOrigin.transform.forward;
        Vector3 newHeadForward = new Vector3(this.transform.forward.x, 0f, this.transform.forward.z);
        Quaternion rot = Quaternion.AngleAxis(i , Vector3.up);
        Vector3 dir = (rot * newHeadForward);
        //Vector3 dir =  rot * Quaternion.LookRotation(this.transform.forward);
        //Debug.Log(dir);
        //Vector3 newvis = new Vector3(visionOrigin.transform.position.x, visionOrigin.transform.position.y, visionOrigin.transform.position.z);
        //Debug.DrawLine(visionOrigin.transform.position, dir, Color.green);
        
        //Debug.Log("")
        Vector3 rayor = this.transform.position + new Vector3(0f, 1.5f, 0f);
        Debug.DrawRay(rayor, this.transform.forward * 30f, Color.red);
        Ray vision = new Ray(rayor, dir);
        if(Physics.Raycast(vision, out hit)) {
            //Physics.cast
            Color color = Color.blue;
            Debug.DrawRay(rayor, dir * 30f, color);
            if(hit.collider.gameObject == target){
                return true;
            }
        }
        return false;
    }

    private enum AIState
    {
        ChasingPlayer,
        Wandering,
        OnASpot,
        InRangeOfPlayer
    };
}