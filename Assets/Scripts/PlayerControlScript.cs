using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlScript : MonoBehaviour
{
    public float speed = 3;
    public GameObject ball;
    public GameObject followCamera;
    public GameObject aimCamera;
    public GameObject ballHolder;

    private Animator anim;
    private float movementX = 0f;
    private float movementY = 0f;
    private bool hasBall = false;

    private Rigidbody rb, ballRb;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool attemptJump;
    private bool slowTime = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ballRb = ball.GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(this.transform.position, this.transform.forward * 10, Color.red);

        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            attemptJump = false;
        }

        Vector3 movementVector = new Vector3(movementX, 0.0f, movementY);
        controller.Move(movementVector * Time.deltaTime * speed);

        if (hasBall)
        {
            ball.transform.position = ballHolder.transform.position;
            ball.transform.rotation = ballHolder.transform.rotation;
        }
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
        anim.SetFloat("velx", movementX);
        anim.SetFloat("vely", movementY);
    }

    void OnYLook(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
    }

    void OnLeftAttack()
    {
        if (hasBall)
        {
            Vector3 forward = transform.forward;
            forward.y = 1;
            ballRb.AddForce(forward * 30, ForceMode.VelocityChange);
            hasBall = false;
            ballRb.detectCollisions = true;
        }
    }

    void OnRightAttack()
    {
        if (aimCamera.active == false)
        {
            followCamera.SetActive(false);
            aimCamera.SetActive(true);
        } else
        {
            followCamera.SetActive(true);
            aimCamera.SetActive(false);
        }
        
    }

    void OnPowerUps()
    {
        if (!slowTime)
        {
            Time.timeScale = 0.5f;
            slowTime = true;
        }
        else
        {
            Time.timeScale = 1f;
            slowTime = false;
        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Ball"))
        {
            ballRb.detectCollisions = false;
            ballRb.velocity = Vector3.zero;
            ballRb.angularVelocity = Vector3.zero;
            hasBall = true;
        }
    }
}
