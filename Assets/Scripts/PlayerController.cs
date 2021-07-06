using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody rb;
    private float movementX;

    private float movementY;

    //public TextMeshProUGUI countText;
    private Animator anim;
    private Collider collider;
    public float forwardMaxSpeed;
    private Camera cam;

    public Transform spine;
    public float fallMultiplier;
    public float lowJumpMultiplier;
    private bool jumping = false;
    private bool isGrounded = true;
    private bool attacking = false;
    private Vector3 directionFacing;
    private Vector3 pointerDirection;
    private Vector2 mousepos;
    private bool isDashing = false;
    public float attackDashStrength;
    public float dashForceDelay;

    private bool dashImpulsing = false;
    // Start is called before the first frame update

    //NEW FROM MULTIPLAYER
    private PlayerConfiguration playerConfig;
    private SkinnedMeshRenderer playerMesh;

    private InputActions controls;
    private InputAction.CallbackContext ctx;

    public Transform originalPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        collider = GetComponent<Collider>();
        controls = new InputActions();
        SkinnedMeshRenderer[] mrs = this.GetComponentsInChildren<SkinnedMeshRenderer>();

        playerMesh = mrs[1];
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    public void InitializePlayer(PlayerConfiguration pc)
    {
        playerConfig = pc;
        playerMesh.material = pc.PlayerMaterial;
        playerConfig.Input.onActionTriggered += Input_onActionTriggered;
    }

    private void Input_onActionTriggered(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //Debug.Log("ACTION " + obj.action.name + " " + obj.action.activeControl.device.name);
        if (obj.action.name == controls.Player.Move.name)
        {
            this.OnMove(obj.ReadValue<Vector2>());
        }

        if (obj.action.name == controls.Player.YLook.name)
        {
            this.OnYLook(obj.ReadValue<Vector2>(), obj);
        }

        if (obj.action.name == controls.Player.LeftAttack.name)
        {
            Debug.Log("   " + obj.started + " " + obj.performed);

            if (obj.started)
            {
                this.OnLeftAttack();
            }
        }

        if (obj.action.name == controls.Player.Jump.name)
        {
            if (obj.started)
            {
                this.OnJump(obj.ReadValue<float>());
            }
        }

        if (obj.action.name == controls.Player.YLook.name)
        {
            this.OnYLook(obj.ReadValue<Vector2>(), obj);
        }

        if (obj.action.name == controls.Player.RightAttack.name)
        {
            if (obj.started)
            {
                this.OnRightAttack();
            }
        }
    }

    /*void SetCountText()
    {
        countText.text = "Count " + count.ToString();
        if(count >= 6)
        {
        }
    }*/
    // Update is called once per frame
    void OnMove(Vector2 movementVector)
    {
        //Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;

        //Debug.Log("onMove " + movementY.ToString());
    }

    void OnYLook(Vector2 look, InputAction.CallbackContext ctx)
    {
        mousepos = look;
        this.ctx = ctx;
        //Debug.Log("YLOOK: " + mousepos.ToString());
    }

    void OnLeftAttack()
    {
        Debug.Log("LATTACK");
        anim.SetTrigger("lattack");
        StartCoroutine(Dash());
    }


    void OnRightAttack()
    {
        anim.SetTrigger("rattack");
        StartCoroutine(Dash());
    }

    // Update is called once per frame
    void OnJump(float input)
    {
        //anim.SetTrigger("Jump");
        jumping = Convert.ToBoolean(input);
        if (jumping && isGrounded)
        {
            rb.AddForce(Vector3.up * 10 * speed, ForceMode.VelocityChange);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movementVector = new Vector3(movementX, 0.0f, movementY);

        //Debug.Log("MovementVect: " + rb.velocity.magnitude.ToString() );
        if (movementVector == Vector3.zero || dashImpulsing)
        {
            if (Math.Abs(rb.velocity.x) > 0.1f && isGrounded || Math.Abs(rb.velocity.z) > 0.1f && isGrounded)
            {
                //Debug.Log("STOPPING: " + rb.velocity.magnitude.ToString() );
                rb.AddForce(rb.velocity * -0.4f, ForceMode.Impulse);
            }
            else
            {
                if (Math.Abs(rb.velocity.x) > 0.1f || Math.Abs(rb.velocity.z) > 0.1f)
                {
                    //Debug.Log("STOPPING: " + rb.velocity.magnitude.ToString() );
                    rb.AddForce(rb.velocity * -0.6f, ForceMode.Impulse);
                }
            }
        }
        else
        {
            if (isGrounded && rb.velocity.magnitude <= forwardMaxSpeed)
            {
                //Debug.Log("Adding force: " + movementVector.ToString() + " " + speed);
                rb.AddForce(movementVector * speed, ForceMode.VelocityChange);
            }
        }

        if (!isDashing)
        {
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * (Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
            }
            else if (rb.velocity.y < 7f)
            {
                rb.velocity += Vector3.up * (Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
            }
            else if (rb.velocity.y > 0 && !jumping)
            {
                rb.velocity += Vector3.up * (Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime);
            }
        }

        if (dashImpulsing)
        {
            dashImpulsing = false;
            rb.AddForce(transform.rotation * Vector3.forward * attackDashStrength, ForceMode.Impulse);
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        rb.useGravity = false;
        yield return new WaitForSeconds(dashForceDelay);
        dashImpulsing = true;
        rb.useGravity = true;
        isDashing = false;
    }

    void Update()
    {
        anim.SetFloat("vely", Mathf.Max(Mathf.Abs(rb.velocity.z), Mathf.Abs(rb.velocity.y)));
        isGrounded = Physics.Raycast(collider.bounds.center, Vector3.down, collider.bounds.extents.y + 0.1f);

        //if(GetComponent<PlayerInput>().currentControlScheme.ToString() != "Gamepad" ){
        //Debug.Log(ctx);

        if (ctx.action != null && ctx.action.activeControl != null && ctx.action.activeControl.device.name == "Mouse")
        {
            pointerDirection = cam.ScreenToWorldPoint(new Vector3(mousepos.x, mousepos.y, 1));
            float t = cam.transform.position.y / (cam.transform.position.y - pointerDirection.y);
            directionFacing =
                new Vector3(t * (pointerDirection.x - cam.transform.position.x) + cam.transform.position.x,
                    rb.transform.position.y,
                    t * (pointerDirection.z - cam.transform.position.z) + cam.transform.position.z);
            transform.LookAt(directionFacing, Vector3.up);
        }
        else
        {
            if (Math.Abs(mousepos.x) >= .1f & Math.Abs(mousepos.y) >= .1f)
            {
                directionFacing = new Vector3(mousepos.x, 0, mousepos.y);
                transform.rotation = Quaternion.LookRotation(directionFacing);
            }
        }

        //anim.SetFloat("velx", rb.velocity.x  );
        // anim.SetFloat("velx", inputTurn);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            //count++;
            //SetCountText(); 
        }
    }
}