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
    public Camera cam;
    
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

    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    static Vector3 moveDir = new Vector3(0f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        collider = GetComponent<Collider>();
    }

    /*void SetCountText()
    {
        countText.text = "Count " + count.ToString();
        if(count >= 6)
        {
        }
    }*/
    // Update is called once per frame
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
        
        //Debug.Log("onMove " + movementY.ToString());

    }
    void OnYLook(InputValue look)
    {
        
        mousepos = look.Get<Vector2>();
    
    }   
    void OnLeftAttack(InputValue movementValue)
    {
        anim.SetTrigger("lattack");
        StartCoroutine(Dash());
    }
    

    void OnRightAttack(InputValue movementValue)
    {
        anim.SetTrigger("rattack");
        StartCoroutine(Dash());
    }
    // Update is called once per frame
    void OnJump(InputValue input)
    {
        //anim.SetTrigger("Jump");
        jumping = Convert.ToBoolean(input.Get<float>());
        if(jumping && isGrounded){
            rb.AddForce(Vector3.up * 10 * speed, ForceMode.VelocityChange);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movementVector = new Vector3(movementX, 0.0f, movementY).normalized;

        if (movementVector.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(movementVector.x, movementVector.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            //float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        }

        else
            moveDir = new Vector3(0f, 0f, 0f);


        //Debug.Log("MovementVect: " + rb.velocity.magnitude.ToString() );
        if (moveDir == Vector3.zero){
            if(Math.Abs(rb.velocity.x) > 0.1f || Math.Abs(rb.velocity.z) > 0.1f && isGrounded){
                //Debug.Log("STOPPING: " + rb.velocity.magnitude.ToString() );
                rb.AddForce(rb.velocity * -0.4f , ForceMode.Impulse);
            }
        } else {
            if(isGrounded && rb.velocity.magnitude <= forwardMaxSpeed)  {
            //Debug.Log("Adding force: " + movementVector.ToString() + " " + speed);
                rb.AddForce(moveDir * speed, ForceMode.VelocityChange);
            }
        }
        if(!isDashing){
            if(rb.velocity.y < 0){
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier  - 1) * Time.deltaTime;
            } else if(rb.velocity.y < 7f){
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier  - 1) * Time.deltaTime;
            } else if (rb.velocity.y > 0 && !jumping) {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier  - 1) * Time.deltaTime;
            }
        }

        if(dashImpulsing){
            dashImpulsing = false;
            rb.AddForce(transform.rotation * Vector3.forward * attackDashStrength, ForceMode.Impulse);
        }
    }
    IEnumerator Dash(){
        isDashing = true;
        rb.useGravity = false;
        yield return new WaitForSeconds(dashForceDelay);
        dashImpulsing = true;
        rb.useGravity = true;
        isDashing = false;
    }
    void Update() 
    {
        anim.SetFloat("vely", rb.velocity.z);
        isGrounded = Physics.Raycast(collider.bounds.center, Vector3.down, collider.bounds.extents.y + 0.1f);

        //pointerDirection = cam.ScreenToWorldPoint(new Vector3(mousepos.x, mousepos.y, 1));
        //float t = cam.transform.position.y / (cam.transform.position.y - pointerDirection.y);
        //directionFacing = new Vector3(t * (pointerDirection.x - cam.transform.position.x) + cam.transform.position.x, rb.transform.position.y  , t * (pointerDirection.z - cam.transform.position.z) + cam.transform.position.z);
        //transform.LookAt(directionFacing, Vector3.up);

        //Debug.Log("Left: " + Vector3.left);
        //Debug.Log("Up: " + Vector3.up);
        //Debug.Log("directionFacing: " + directionFacing);


        //float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        //transform.rotation = Quaternion.Euler(0f, angle, 0f);
        // moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

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
