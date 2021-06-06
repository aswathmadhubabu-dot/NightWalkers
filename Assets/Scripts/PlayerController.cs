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
        anim.SetFloat("vely", movementY);
        anim.SetFloat("velx", movementX);
        Debug.Log("onMove " + movementY.ToString());

        //rb.MovePosition(rb.position + this.transform.forward * movementY */* Time.deltaTime */ forwardMaxSpeed);

    }
    void OnYLook(InputValue look)
    {
        
        float ychange = look.Get<float>();
        Debug.Log("onyLook " + ychange);
        //float RotationX = HorizontalSensitivity * this.InputsManager.MouseX * Time.deltaTime;
        float RotationY = ychange ;
        Debug.Log("onLooktransform " + RotationY);

        // lock players x axis rotation when aiming vertically so feet stay on ground
        spine.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, RotationY, transform.rotation.eulerAngles.z); 
        
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
        rb.AddForce(new Vector3(0.0f, 0.4f, 0.0f) * speed, ForceMode.Impulse);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movementVector = new Vector3(movementX, 0.0f, movementY);
        if(movementVector != Vector3.zero )  {
            Debug.Log("Adding force: " + movementVector.ToString() + " " + speed);
            rb.AddForce(movementVector * speed);
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
