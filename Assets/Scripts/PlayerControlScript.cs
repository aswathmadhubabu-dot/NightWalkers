using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerControlScript : MonoBehaviour
{
    public float speed = 3.0f;
    public float followCameraDistance = 3.5f;
    public float aimCameraDistance = 1.1f;
    public float throwBallForce = 7f;
    public float slopeForce = 20f;
    public float slopeForceRayLenth = 1.5f;

    public GameObject ball;
    public GameObject ballHolder;
    public CinemachineVirtualCamera camera;
    private Cinemachine3rdPersonFollow thirdPersonFollowCamera;
    private Camera mainCamera;
    private BallPredictionScript ballProjectionPredictor;

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

    [HideInInspector] public bool hasBall = false;
    private bool slowTime = false;
    [HideInInspector] public bool isAiming = false;

    private GameObject rightHand;
    private GameObject leftHand;

    public float ballCloseEnoughForPickDistance = 8f;
    double ballCloseEnoughForPickAngleDegree = 0.8;

    private Vector3 initialBallVelocity;
    private Vector3 initialBallAngularVelocity;

    private Boolean enablePlayer = true;

    public void EnablePlayer(Boolean enable)
    {
        enablePlayer = enable;
    }

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
        mainCamera = Camera.main;
        ballProjectionPredictor = GetComponent<BallPredictionScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enablePlayer)
        {
            MovePlayer();

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

            if (inputManager.PickUpBallTriggeredThisFrame() &&
                ballDistanceFromPlayer <= ballCloseEnoughForPickDistance &&
                isFacingBall)
            {
                pickUpBall();
            }

            if (inputManager.DropUpBallTriggeredThisFrame())
            {
                DropBall();
            }
        }
    }

    public void MakePlayerDance()
    {
        anim.SetTrigger("dance");
    }

    public void MakePlayerDefeated()
    {
        //anim.SetTrigger("defeated");
    }

    void FixedUpdate()
    {
        if (hasBall && isAiming)
        {
            //ballProjectionPredictor.predictBallPath(throwBallForce);
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
        Vector2 move2d = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(move2d.x, 0.0f, move2d.y);
        forwardVel = Mathf.Lerp(forwardVel, move.z, Time.deltaTime * 5);

        turnVel = Mathf.Lerp(turnVel, move.x,
            Time.deltaTime * 5);
        move.y = 0f;

        anim.SetFloat("velx", turnVel);
        anim.SetFloat("vely", forwardVel);

        if (move != Vector3.zero && OnSlope())
        {
            controller.Move(Vector3.down * controller.height / 2 * slopeForce * Time.deltaTime);
        }
    }

    private bool OnSlope()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, controller.height / 2 * slopeForceRayLenth))
        {
            if (hit.normal != Vector3.up)
            {
                return true;
            }
        }
        return false;
    }

    public void ThrowBall()
    {
        if (hasBall)
        {
            // anim.SetTrigger("throw");
            DisableBallKinematics();
            Vector3 forward = this.transform.forward;
            forward.y = 0.1f;
            ballRb.AddForce(forward * throwBallForce, ForceMode.Impulse);
            EventManager.TriggerEvent<ThrowBallEvent, Vector3>(ball.transform.position);
            hasBall = false;
            ball.transform.parent = null;
            
            StartCoroutine(ExecuteAfterTime(0.3f, () =>
            {            
                ball.GetComponent<Collider>().isTrigger = false;
            }));
        }
    }

    private Boolean isCoroutineExecuting = false;
    
    IEnumerator ExecuteAfterTime(float time, Action task)
    {
        if (isCoroutineExecuting)
            yield break;
        isCoroutineExecuting = true;
        yield return new WaitForSeconds(time);
        task();
        isCoroutineExecuting = false;
    }

    void OnZoom()
    {
        if (!isAiming)
        {
            isAiming = true;
            thirdPersonFollowCamera.CameraDistance = aimCameraDistance;
            thirdPersonFollowCamera.ShoulderOffset.x = 1.5f;
        }
        else
        {
            isAiming = false;
            thirdPersonFollowCamera.CameraDistance = followCameraDistance;
            thirdPersonFollowCamera.ShoulderOffset.x = 0;
            //ballProjectionPredictor.reset();
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