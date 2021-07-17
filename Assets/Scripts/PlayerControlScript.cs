using System;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerControlScript : MonoBehaviour
{
    public float speed = 3.0f;
    public float followCameraDistance = 3.5f;
    public float aimCameraDistance = 1.1f;
    public float throwBallForce = 7f;

    public GameObject ball;
    public GameObject ballHolder;
    public CinemachineVirtualCamera camera;
    private Cinemachine3rdPersonFollow thirdPersonFollowCamera;
    private Animator anim;

    private InputManager inputManager;

    // Jumping stuff
    private bool groundedPlayer;
    private float jumpHeight = 2.0f;
    private float gravityValue = -9.81f;
    private float turnVel;
    private float forwardVel;
    private int jumpsRemaining = 2;


    private Rigidbody rb, ballRb;
    private CharacterController controller;
    private Vector3 playerVelocity;

    [HideInInspector]
    public bool hasBall = false;
    private bool slowTime = false;
    private bool isAiming = false;

    private GameObject rightHand;
    private GameObject leftHand;

    public float ballCloseEnoughForPickDistance = 2f;
    double ballCloseEnoughForPickAngleDegree = 0.2;

    private Vector3 initialBallVelocity;
    private Vector3 initialBallAngularVelocity;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ballRb = ball.GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        inputManager = InputManager.Instance;
        thirdPersonFollowCamera = camera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        rightHand = GameObject.FindWithTag("rightHand");
        leftHand = GameObject.FindWithTag("leftHand");
        initialBallVelocity = ballRb.velocity;
        initialBallAngularVelocity = ballRb.angularVelocity;
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
            // ball.transform.position = ballHolder.transform.position;
            ball.transform.position = (rightHand.transform.position + leftHand.transform.position) / 2;
            ball.transform.parent = rightHand.transform;
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

        // Use this is we need to click P button and manually pick up the ball - inputManager.PickUpBallTriggeredThisFrame() 

        // if (ballDistanceFromPlayer <= ballCloseEnoughForPickDistance && isFacingBall)
        // {
        //     pickUpBall();
        // }

        if (inputManager.PickUpBallTriggeredThisFrame() && ballDistanceFromPlayer <= ballCloseEnoughForPickDistance &&
            isFacingBall)
        {
            pickUpBall();
        }

        if (inputManager.DropUpBallTriggeredThisFrame())
        {
            DropBall();
        }
    }

    void DropBall()
    {
        print("Drop ball");
        if (hasBall)
        {
            print("Dropping ball");
            anim.SetBool("carry", false);
            DisableBallKinematics();
            Vector3 forward = this.transform.forward;
            forward.y = 0.1f;
            ballRb.AddForce(forward * 0, ForceMode.Impulse);
            hasBall = false;
            ball.transform.parent = null;
        }
    }

    private void DisableBallKinematics() => ballRb.isKinematic = false;

    void pickUpBall()
    {
        anim.SetTrigger("pickUpBall");
        print("Pick up ball");
        ballRb.velocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;
        ballRb.isKinematic = true;
        hasBall = true;
        anim.SetBool("carry", true);
    }

    void MovePlayer()
    {
        //if (isAiming)
        //{
        //    float xvel = anim.GetFloat("velx") - (2 * Time.deltaTime);
        //    float yvel = anim.GetFloat("vely") - (2 * Time.deltaTime);
        //    anim.SetFloat("velx", Mathf.Max(0, xvel));
        //    anim.SetFloat("vely", Mathf.Max(0, yvel));
        //    return;
        //}

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
        // groundedPlayer = controller.isGrounded;
        // if (groundedPlayer && playerVelocity.y < 0)
        // {
        //     playerVelocity.y = 0f;
        //     if (jumpsRemaining != 2)
        //     {
        //         jumpsRemaining = 2;
        //     }
        // }
        //
        // if (inputManager.PlayerJumpedThisFrame() && (groundedPlayer || jumpsRemaining > 0))
        // {
        //     anim.SetTrigger("isJumping");
        //     playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        //     jumpsRemaining -= 1;
        // }
        //
        // playerVelocity.y += gravityValue * Time.deltaTime;
        // controller.Move(playerVelocity * Time.deltaTime);

        if (inputManager.PlayerJumpedThisFrame())
        {
            print("Jumping");
            anim.SetTrigger("jump");
        }
    }

    public void ThrowBall()
    {
        if (hasBall)
        {
            DisableBallKinematics();
            Vector3 forward = this.transform.forward;
            forward.y = 0.1f;
            ballRb.AddForce(forward * throwBallForce, ForceMode.Impulse);
            hasBall = false;
            ball.transform.parent = null;
        }
    }

    void OnZoom()
    {
        if (!isAiming)
        {
            isAiming = true;
            thirdPersonFollowCamera.CameraDistance = 1.1f;
        }
        else
        {
            isAiming = false;
            thirdPersonFollowCamera.CameraDistance = 3.5f;
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
}