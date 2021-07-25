using UnityEngine;

public class AvoidCollision : MonoBehaviour
{
    private Rigidbody mBody;

    void Awake()
    {
        mBody = this.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 horizontalMove = mBody.velocity;
        horizontalMove.y = 0;
        float distance = horizontalMove.magnitude * Time.fixedDeltaTime;
        horizontalMove.Normalize();
        RaycastHit hit;

        if (mBody.SweepTest(horizontalMove, out hit, distance))
        {
            mBody.velocity = new Vector3(0, mBody.velocity.y, 0);
        }
    }
}