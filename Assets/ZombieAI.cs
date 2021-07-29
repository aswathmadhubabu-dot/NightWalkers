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
    private bool isDetecting = false;
    private Vector3 wanderpoint; 
    private NavMeshAgent agent;
    private Renderer render;
    public float roamRadius = 7f;
    private Animator animator;
    private bool nearPlayer = false;
    public float minDistance = 3.5f;
    public float damping = 1.0f;
    public float deathCounter = 0.0f;
    private float loseThreshold = 10f;
    private float loseTimer = 0f;
    private Vector3 jitterVector;
    private float attackTime = 0f;

    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        render = GetComponent<Renderer>();
        vr = playerTransform.GetComponent<VelocityReporter>();
        wanderpoint = RandomWanderPoint();
        animator = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void FixedUpdate()

    {
        var vectorToPlayer = playerTransform.transform.position - transform.position;
        vectorToPlayer.y = 0;
        var distanceToPlayer = vectorToPlayer.magnitude;
        //Debug.Log("Striking Distance: " + distanceToPlayer);
        //Debug.Log("Death Counter: " + deathCounter);

        if (distanceToPlayer < minDistance)
        {
            nearPlayer = true;
        }
        else
        {
            nearPlayer = false;
        }

        if (nearPlayer == false)
        {
            if (isInsight)
            {
                //Debug.Log("Striking Distance: " + Vector3.Distance(playerTransform.transform.position, transform.position));
                agent.SetDestination(playerTransform.transform.position);
                animator.SetBool("Attack", false);
                animator.SetBool("Chase", true);
                agent.speed = ChaseSpeed;
                if (!isDetecting)
                {
                    loseTimer += Time.deltaTime;
                    if (loseTimer >= loseThreshold)
                    {
                        isInsight = false;
                        loseTimer = 0;
                    }
                }

            }
            else
            {
                Wander();
                animator.SetBool("Chase", false);
                animator.SetBool("Attack", false);
                agent.speed = WanderSpeed;

            }
            SearchForPlayer();
        }
        else
        {
            if (distanceToPlayer < 0.35f)
            {
                jitterVector = new Vector3(playerTransform.transform.position.x + 1f, playerTransform.transform.position.y, playerTransform.transform.position.z);
                animator.SetBool("Attack", false);
                agent.SetDestination(jitterVector);
                animator.SetBool("Chase", true);

            }
            else
            {
                lookAt();
                animator.SetBool("Chase", false);
                if (!animator.GetBool("Attack"))
                {
                    animator.SetFloat("AttackType", Random.Range(0, 3));
                }
                
                //Debug.Log("TIME:" + Time.realtimeSinceStartup.ToString());
                //Debug.Log("TIME DIFF:" + (Time.realtimeSinceStartup - attackTime).ToString());
                if ((Time.realtimeSinceStartup - attackTime) > 1f)
                {
                    animator.SetBool("Attack", true);
                    EventManager.TriggerEvent<ZombieAttackEvent, Vector3>(transform.position);
                    Debug.Log("Hit Player");
                    playerTransform.GetComponent<HealthController>().TakeDamage(5, this.gameObject);
                    attackTime = Time.realtimeSinceStartup;
                }                

                agent.speed = 0f;
            }
        }

    }

    public void SearchForPlayer()
    {
        //Debug.Log("Angle: " + Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(playerTransform.transform.position)));
        if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(playerTransform.transform.position)) < fov / 2)
        {
            //Debug.Log("Distance: " + Vector3.Distance(playerTransform.transform.position, transform.position));
            if (Vector3.Distance(playerTransform.transform.position, transform.position) < viewDistance)
            {
                //Debug.Log("Distance: " + Vector3.Distance(playerTransform.transform.position, transform.position));

                RaycastHit hit;
                if (Physics.Linecast(playerTransform.transform.position, transform.position, out hit, -1))
                {
                    //Debug.Log("What I am hitting: " + hit.transform.gameObject);
                    //Debug.Log("Layer Hititng: " + hit.transform.gameObject.layer);
                    if (hit.collider.gameObject.layer == 10) // This bit hacky may need to change at some point.
                    {
                        isInsight = true;
                        isDetecting = true;
                        loseTimer = 0;

                    } else
                    {
                        isDetecting = false;
                    }
                } else
                {
                    isDetecting = false;
                }
            } else
            {
                isDetecting = false;
            }
        } else
        {
            isDetecting = false;
        }

    }

    public void Wander()
    {
        if (wanderType == WanderType.Random)
        {
            //Debug.Log("Wander point: " + wanderpoint);
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

    void lookAt()
    {
        var rotation = Quaternion.LookRotation(playerTransform.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.gameObject.CompareTag("Ball"))
        {
            Debug.Log("Dath Counter:" + deathCounter);
            if (deathCounter > 0)
            {

                deathCounter = deathCounter - 1;
                Debug.Log(deathCounter);
                animator.SetBool("BallAttack", true);

            } else
            {
                animator.SetBool("Chase", false);
                animator.SetBool("Attack", false);
                animator.SetTrigger("die");
                EventManager.TriggerEvent<ZombieDeathEvent, Vector3>(transform.position);
                Destroy(gameObject.GetComponent<CapsuleCollider>());
                Destroy(gameObject.GetComponent<Rigidbody>());
                agent.speed = 0;
                agent.angularSpeed = 0;
                Destroy(gameObject.GetComponent<MeshCollider>());
                Destroy(gameObject.GetComponent<MeshRenderer>());
                gameObject.GetComponent<ZombieAI>().enabled = false;
            }
    
            

        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.transform.gameObject.CompareTag("Ball"))
        {
            animator.SetBool("BallAttack", false);
            RandomWanderPoint();
            animator.SetBool("Chase", true);
        }
    }

    IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

    }
}
