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
    public float forwardMaxSpeed;
    public Camera cam;

    public Transform spine;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        //SetCountText();

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

        Debug.Log("onMove " + movementY.ToString());

        //rb.MovePosition(rb.position + this.transform.forward * movementY */* Time.deltaTime */ forwardMaxSpeed);

    }
    void OnYLook(InputValue look)
    {
        
        Vector2 mousepos = look.Get<Vector2>();
        Debug.Log("mousepos " + mousepos.ToString() );
        Vector3 point = cam.ScreenToWorldPoint(new Vector3(mousepos.x, mousepos.y, 1));
        float t = cam.transform.position.y / (cam.transform.position.y - point.y);
        Vector3 finalPoint = new Vector3(t * (point.x - cam.transform.position.x) + cam.transform.position.x, 0.4f, t * (point.z - cam.transform.position.z) + cam.transform.position.z);

        transform.LookAt(finalPoint, Vector3.up);
    }   
    void OnLeftAttack(InputValue movementValue)
    {
        anim.SetTrigger("lattack");
    }
    

    void OnRightAttack(InputValue movementValue)
    {
        anim.SetTrigger("rattack");
    }
    // Update is called once per frame
    void OnJump(InputValue movementValue)
    {
        //anim.SetTrigger("Jump");
        rb.AddForce(new Vector3(0.0f, 0.4f, 0.0f) * 20 * speed, ForceMode.VelocityChange);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        anim.SetFloat("vely", rb.velocity.z  );
        anim.SetFloat("velx", rb.velocity.x  );

        Vector3 movementVector = new Vector3(movementX, 0.0f, movementY);
        if(movementVector != Vector3.zero )  {
            Debug.Log("Adding force: " + movementVector.ToString() + " " + speed);
            rb.AddForce(movementVector * speed, ForceMode.VelocityChange);
        }
    }
    void Update() 
    {
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
