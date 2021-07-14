using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class PlayerControlScript : MonoBehaviour
{
    public float speed = 3.0f;
    private bool groundedPlayer;
    private float jumpHeight = 2.0f;
    private float gravityValue = -9.81f;
    private float turnVel;
    private float forwardVel;

    public GameObject ball;
    public GameObject followCamera;
    public GameObject aimCamera;
    public GameObject ballHolder;

    private Animator anim;
    private Collider collider;
    private float movementX = 0f;
    private float movementY = 0f;
    private bool hasBall = false;
    private bool ballPosSet = false;

    private Rigidbody rb, ballRb;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool attemptJump;
    private bool slowTime = false;
    private int jumpsRemaining = 2;
    private InputManager inputManager;
    private Transform cameraTransform;

    public float ballCloseEnoughForPickDistance = 2f;
    double ballCloseEnoughForPickAngleDegree = 0.2;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ballRb = ball.GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        collider = GetComponent<BoxCollider>();
        inputManager = InputManager.Instance;
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();

        HandleJump();
        /*
        Quaternion characterRotation = cameraTransform.rotation;
        characterRotation.x = 0;
        characterRotation.z = 0;
        transform.rotation = characterRotation;
        */

        if (hasBall)
        {
            ball.transform.position = ballHolder.transform.position;
        }

        if (inputManager.AttackedThisFrame())
        {
            if (hasBall)
            {
                anim.SetBool("carry", false);
                anim.SetTrigger("throw");
            }
        }

        if (inputManager.ZoomedThisFrame())
        {
            OnZoom();
        }

        if (inputManager.PowerUpTriggeredThisFrame())
        {
            SlowTime();
        }

        float ballDistanceFromPlayer = float.MaxValue;


        var ballPosition = ball.transform.position;
        var characterPosition = transform.position;

        ballDistanceFromPlayer = Vector3.Distance(characterPosition, ballPosition);

        Vector3 dir = (ballPosition - characterPosition).normalized;
        float dot = Vector3.Dot(dir, transform.forward);

        var isFacingBall = Math.Abs(dot - 1.0) < ballCloseEnoughForPickAngleDegree;

        if (inputManager.PickupBallTriggeredThisFrame() && ballDistanceFromPlayer <= ballCloseEnoughForPickDistance
                                                        && isFacingBall)
        {
            print(Math.Abs(dot - 1.0) < 0);
            anim.SetTrigger("pickUpBall");
            print("Pick up ball");
        }
    }

    void MovePlayer()
    {
        Vector2 move2d = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(move2d.x, 0.0f, move2d.y);
        forwardVel = Mathf.Lerp(forwardVel, move.z, Time.deltaTime * 5);

        turnVel = Mathf.Lerp(turnVel, move.x,
            Time.deltaTime * 5);
        //move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        move.y = 0f;

        //controller.Move(move * Time.deltaTime * speed);
        anim.SetFloat("velx", turnVel);
        anim.SetFloat("vely", forwardVel);
    }

    void HandleJump()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            if (jumpsRemaining != 2)
            {
                jumpsRemaining = 2;
            }
        }

        if (inputManager.PlayerJumpedThisFrame() && (groundedPlayer || jumpsRemaining > 0))
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            jumpsRemaining -= 1;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void ThrowBall()
    {
        if (hasBall)
        {
            ballRb.isKinematic = false;
            Vector3 forward = cameraTransform.forward;
            forward.y = 0.1f;
            print("Throwing ball");
            ballRb.AddForce(forward * 7, ForceMode.Impulse);
            hasBall = false;
        }
    }

    void OnZoom()
    {
        if (aimCamera.active == false)
        {
            followCamera.SetActive(false);
            aimCamera.SetActive(true);
        }
        else
        {
            followCamera.SetActive(true);
            aimCamera.SetActive(false);
        }
    }

    void SlowTime()
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
        // if (collider.CompareTag("Ball"))
        // {
        //     ballRb.velocity = Vector3.zero;
        //     ballRb.angularVelocity = Vector3.zero;
        //     ballRb.isKinematic = true;
        //     hasBall = true;
        //     anim.SetBool("carry", true);
        // }
    }
}