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
        Debug.Log("YLOOK: " + mousepos.ToString());

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
        Vector3 movementVector = new Vector3(movementX, 0.0f, movementY);

        //Debug.Log("MovementVect: " + rb.velocity.magnitude.ToString() );
        if(movementVector == Vector3.zero || dashImpulsing ){
            if(Math.Abs(rb.velocity.x) > 0.1f && isGrounded || Math.Abs(rb.velocity.z) > 0.1f && isGrounded){
                //Debug.Log("STOPPING: " + rb.velocity.magnitude.ToString() );
                rb.AddForce(rb.velocity * -0.4f , ForceMode.Impulse);
            } else {
                if(Math.Abs(rb.velocity.x) > 0.1f || Math.Abs(rb.velocity.z) > 0.1f){
                    //Debug.Log("STOPPING: " + rb.velocity.magnitude.ToString() );
                    rb.AddForce(rb.velocity * -0.6f , ForceMode.Impulse);
                }
            }

        } else {
            if(isGrounded && rb.velocity.magnitude <= forwardMaxSpeed)  {
            //Debug.Log("Adding force: " + movementVector.ToString() + " " + speed);
                rb.AddForce(movementVector * speed, ForceMode.VelocityChange);
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

        if(GetComponent<PlayerInput>().currentControlScheme.ToString() != "Gamepad" ){
                pointerDirection = cam.ScreenToWorldPoint(new Vector3(mousepos.x, mousepos.y, 1));Debug.Log(pointerDirection);
                float t = cam.transform.position.y / (cam.transform.position.y - pointerDirection.y);
                directionFacing = new Vector3(t * (pointerDirection.x - cam.transform.position.x) + cam.transform.position.x, rb.transform.position.y  , t * (pointerDirection.z - cam.transform.position.z) + cam.transform.position.z);
                transform.LookAt(directionFacing, Vector3.up);
        } else {
            if(Math.Abs(mousepos.x) >= .1f & Math.Abs(mousepos.y) >= .1f){
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
