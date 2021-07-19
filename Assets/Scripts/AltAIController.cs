using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

public class AltAIController : MonoBehaviour
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
    void Start()
    {
        navmesh = GetComponent<UnityEngine.AI.NavMeshAgent>();  
        anim  = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        vr = target.GetComponent<VelocityReporter>();
        navmesh.updatePosition = false;
        navmesh.updateRotation = false;
        setNextWaypoint();
        firing = false;
        
    }
    void setNextWaypoint(){
        Debug.Log("Bored here, moving to new point");
        //navmesh.stoppingDistance = 1;
        navmesh.SetDestination( this.transform.position +  new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f)));
        //TODO CHECK IF DESTINATION IS IN FIELD. OTHERWISE GENERATE A NEW ONE.
        aiState = AIState.Wandering;
    }
    void OnAnimatorMove()
    {
        //this.transform.position = navmesh.nextPosition;
        this.transform.position = anim.rootPosition;
        this.transform.rotation = anim.rootRotation;
       // agent.nextPosition = this.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 dist = target.transform.position - this.transform.position;
        bool stopping;
        switch (aiState) {

            case AIState.OnASpot:
                if(Time.time - spotArrivalTime  > timeInSpots){
                    setNextWaypoint();
                }
                CheckIfRobotSeesPlayer(dist);
            break;
            case AIState.Wandering:
                stopping = false;
                if(navmesh.remainingDistance < 3 && !navmesh.pathPending){
                    stopping = true;
                    //Keep wandering around
                }
                MoveRootMotionRobot(stopping);
                if(navmesh.remainingDistance < 1 && !navmesh.pathPending){
                    StopRobot();
                    StayOnSpot();
                }
                CheckIfRobotSeesPlayer(dist);
            break; 
            case AIState.ChasingPlayer:
                
                CalculateDistanceAndChasePlayer(dist);
                stopping = false;
                if(navmesh.remainingDistance < 10){
                    stopping = true;
                }
                MoveRootMotionRobot(stopping);
                
                if(isPlayerInSight()){
                    if(navmesh.remainingDistance < 10 && !navmesh.pathPending){
                        StopRobot();
                        aiState = AIState.InRangeOfPlayer;
                    }
                } else {
                    //Lost Player from Sight.
                    Debug.Log("Lost Player, staying here");
                    StopRobot();
                }
            break;
            case AIState.InRangeOfPlayer:
                anim.SetBool("aiming", true);
                MoveRootMotionRobot(true);

                Debug.Log("Got close to player Shooting! " + navmesh.remainingDistance.ToString());
                
                if(!firing){
                    StartCoroutine(WaitAndFire());
                }

            break;
        }
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
        yield return new WaitForSeconds(1);
        Aim();
        yield return new WaitForSeconds(1);
        Fire();
        //After we have waited 5 seconds print the time again.
        //Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        yield return new WaitForSeconds(1);
        firing = false;
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
    void CheckIfRobotSeesPlayer(Vector3 dist){
        if(dist.magnitude < 35 && isPlayerInSight()){
            //Can see the player, lets chase him!
            aiState = AIState.ChasingPlayer;
            // Play sound here probably
            Debug.Log("Detected player!");
        }
    }
    void StayOnSpot(){
        aiState = AIState.OnASpot;
        spotArrivalTime = Time.time;
    }
    void SetWandering(){
        StopRobot();
        aiState = AIState.Wandering;
    }
    void StopRobot(){
        navmesh.isStopped = true;
        navmesh.ResetPath();
    }

    void MoveRootMotionRobot(bool stopping){
        worldDeltaPosition = navmesh.nextPosition - transform.position;
        groundDeltaPosition.x = Vector3.Dot(transform.right, worldDeltaPosition);
        groundDeltaPosition.y = Vector3.Dot(transform.forward, worldDeltaPosition);
        velocity = (Time.deltaTime > 1e-5f ) ? groundDeltaPosition / Time.deltaTime : Vector2.zero;
        bool moving = velocity.magnitude > 0.025f && navmesh.remainingDistance > navmesh.radius;
 

        if(stopping){
            forwardVel = Mathf.Lerp(forwardVel, 0, Time.deltaTime * 10);
            turnVel = Mathf.Lerp(turnVel,  0, Time.deltaTime * 10);
            if(forwardVel < 0.05){
                forwardVel = 0;
                //Debug.Log("SET0");

            }
            if(turnVel < 0.05){
                turnVel = 0;
            }
        } else {
            forwardVel = velocity.y;
            turnVel = velocity.x;
        }
        //Debug.Log(forwardVel);

        anim.SetBool("moving", moving);
        anim.SetFloat("vely", forwardVel);
        anim.SetFloat("velx", turnVel);
    }
    bool isPlayerInSight()
    {
        Vector3 targetDir = target.transform.position - transform.position;
        float angleToPlayer = (Vector3.Angle(targetDir, transform.forward));
        bool seenPlayer = false;
        if (angleToPlayer >= -70 && angleToPlayer <= 70){
            //If angles are correct, Raycast to see if it can see the char.
            //Debug.Log("Watching around: " + targetDir);
            
            for(float i = -355; i <= -290; i += 5 ){
                seenPlayer = ViewForAngle(i);
            }
            //Ugly hack to prevent GimbalLock
            for(float i = -70; i <= -1; i += 5 ){
                seenPlayer = ViewForAngle(i) && seenPlayer;
            }
        } 
        return seenPlayer;
    }
    bool ViewForAngle(float i){
        RaycastHit hit;
        Vector3 headForward = visionOrigin.transform.forward;
        Vector3 newHeadForward = new Vector3(this.transform.forward.x, 0f, this.transform.forward.z);
        Quaternion rot = Quaternion.AngleAxis(i , Vector3.up);
        if(i > 10){
            Debug.Log("ROT" + rot);
        }
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
            if(i > 10){
                color = Color.green;
            }
            Debug.DrawRay(rayor, dir * 30f, color);
            if(hit.collider.gameObject == target){
                Debug.Log("Can see player");
                return true;
            }
        }
        return false;
    }
    public enum AIState
    {
        ChasingPlayer,
        Wandering,
        OnASpot,
        InRangeOfPlayer
    };

}
