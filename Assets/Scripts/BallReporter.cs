using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallReporter : MonoBehaviour
{
    public double distanceFromGround { get; set; }
    public bool isGrounded { get; set; }

    public float maxDistance = 50f;
    private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, maxDistance))
        {
            if (hit.transform.CompareTag("Ground"))
            {
                distanceFromGround = hit.distance - (this.transform.localScale.y / 2);

                if (distanceFromGround < 0.5)
                {
                    isGrounded = true;
                }
                else
                {
                    isGrounded = false;
                }
            }
        }
    }
}
