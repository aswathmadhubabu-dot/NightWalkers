using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviourScript : MonoBehaviour
{
    [SerializeField] float hitPower = 1f;
    // Start is called before the first frame update
    void OnCollisionEnter(Collision c)
    {
        if (c.impulse.magnitude > 0.25f) {

            GameObject other = c.collider.gameObject;

            if (other.tag == "Player")
            {
                // Check if the ball got hit by a player or just touched a static player 

                var velocity_player = other.GetComponent<VelocityReporter>().velocity;
                var velocity_ball = this.GetComponent<VelocityReporter>().velocity;

                // get the direction from player to collider
                Vector3 collisionNormal = (other.transform.position - transform.position).normalized;

                // get player speed towards the collision
                float ballCollisionSpeed = Vector3.Dot(collisionNormal, velocity_ball);
                if (ballCollisionSpeed < 0) { ballCollisionSpeed = 0; }

                float otherCollisionSpeed = 0f;

                // if other object had velocity towards the collision . . .
                if (velocity_player.magnitude > ballCollisionSpeed)
                {
                    // . . . get the other object's speed towards the collision
                    otherCollisionSpeed = Vector3.Dot(-collisionNormal, velocity_ball + velocity_player);
                }

                //Debug.Log("COLLISION SPEED" + otherCollisionSpeed.ToString() + " " + playerCollisionSpeed.ToString());
                if (otherCollisionSpeed > ballCollisionSpeed)
                {
                    //Debug.Log("ball got hit by player");
                    EventManager.TriggerEvent<PunchBallEvent, Vector3, float>(c.collider.transform.position, c.impulse.magnitude);
                }
                else
                {
                    EventManager.TriggerEvent<HitBallEvent, Vector3, float>(c.contacts[0].point, c.impulse.magnitude);
                    //Debug.Log("player got hit by ball");
                }
                
            }
            else
            {
                EventManager.TriggerEvent<HitBallEvent, Vector3, float>(c.contacts[0].point, c.impulse.magnitude);
            }
        }
            
    }

}