using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    //public TextMeshProUGUI countText;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
         
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
    }

    // Update is called once per frame
    void OnJump(InputValue movementValue)
    {
        rb.AddForce(new Vector3(0.0f, 4.8f, 0.0f), ForceMode.Impulse);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movementVector = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movementVector * speed);
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
