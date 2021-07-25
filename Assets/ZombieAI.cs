using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    // Start is called before the first frame update
    public enum WanderType {  Random, Waypoint};
    public GameObject playerTransform;

    public Transform[] WayPoints;
    public int startPoint;
    public WanderType wanderType;
    private VelocityReporter vr;
    public float WanderSpeed = 1.5f;
    public float ChaseSpeed = 5f;
    public float fov = 180f;
    public float viewDistance = 8f;
    public bool isInsight = false;
    private Vector3 wanderpoint; 
    private NavMeshAgent agent;
    private Renderer render;
    public float roamRadius = 7f;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        render = GetComponent<Renderer>();
        vr = playerTransform.GetComponent<VelocityReporter>();
        wanderpoint = RandomWanderPoint();
        animator = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()

    {


        if (isInsight)
        {
            Debug.Log("Striking Distance: " + Vector3.Distance(playerTransform.transform.position, transform.position));
            agent.SetDestination(playerTransform.transform.position);
            animator.SetBool("Chase", true);
            agent.speed = ChaseSpeed;
        }
        else
        {
            SearchForPlayer();
            Wander();
            animator.SetBool("Chase", false);
            agent.speed = WanderSpeed;

        }

    }

    public void SearchForPlayer()
    {
        Debug.Log("Angle: " + Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(playerTransform.transform.position)));
        if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(playerTransform.transform.position)) < fov / 2)
        {
            Debug.Log("Distance: " + Vector3.Distance(playerTransform.transform.position, transform.position));
            if (Vector3.Distance(playerTransform.transform.position, transform.position) < viewDistance)
            {
                Debug.Log("Distance: " + Vector3.Distance(playerTransform.transform.position, transform.position));

                RaycastHit hit;
                if (Physics.Linecast(playerTransform.transform.position, transform.position, out hit, -1))
                {
                    Debug.Log("What I am hitting: " + hit.transform.gameObject);
                    Debug.Log("Layer Hititng: " + hit.transform.gameObject.layer);
                    if (hit.collider.gameObject.layer == 10) // This bit hacky may need to change at some point.
                    {
                        isInsight = true;

                    }
                }
            }
        }

    }

    public void Wander()
    {
        if (wanderType == WanderType.Random)
        {
            Debug.Log("Wander point: " + wanderpoint);
            if (Vector3.Distance(transform.position, wanderpoint) < 2f)
            {
                wanderpoint = RandomWanderPoint();
            }
            else
            {
                agent.SetDestination(wanderpoint);
            }
        } else
        {
            if (Vector3.Distance(WayPoints[startPoint].position, transform.position) < 5f)
            {
                if (startPoint == WayPoints.Length - 1)
                {
                    startPoint = 0;
                } else
                {
                    startPoint++;
                }
            } else
            {
                agent.SetDestination(WayPoints[startPoint].position);
            }
        }
        
    }

    public Vector3 RandomWanderPoint()
    {
        Vector3 randomPoint = (Random.insideUnitSphere * roamRadius) + transform.position;
        NavMeshHit navhit;
        NavMesh.SamplePosition(randomPoint, out navhit, roamRadius, -1);
        return new Vector3(navhit.position.x, transform.position.y, navhit.position.z);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("COLLIDING COLLIDNG");
        animator.SetBool("Chase", false);
        animator.SetBool("Attack", true);
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("COLLIDING COLLIDNG");
        animator.SetBool("Attack", false);
        animator.SetBool("Chase", true);

    }
}
