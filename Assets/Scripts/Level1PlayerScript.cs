using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerControlScript))]
public class Level1PlayerScript : MonoBehaviour
{

    PlayerControlScript playerScript;
    public float resetTimer = 3.5f;


    private bool ballThrown = false;
    private bool ballWasPickedUp = false;
    private bool waitInitiated = false;
    private bool resetPosition = false;

    public GameObject ball;
    public GameObject player;

    private Vector3 ballStartPosition;
    private Vector3 playerStartPosition;
    private Quaternion playerStartRotation;

    // Start is called before the first frame update
    void Awake()
    {
        playerScript = GetComponent<PlayerControlScript>();
    }

    void Start()
    {
        playerStartPosition = transform.position;
        playerStartRotation = transform.rotation;

        ballStartPosition = ball.transform.position;
        ballStartPosition.y = ballStartPosition.y + 0.5f;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (playerScript.hasBall && !ballWasPickedUp)
        {
            ballWasPickedUp = true;
        }
        else if (ballWasPickedUp && !playerScript.hasBall && !waitInitiated)
        {
            waitInitiated = true;
            StartCoroutine(resetPlayerAndBall(resetTimer));
        }
        if (resetPosition)
        {
            updatePlayerBallTransform();
        }
    }

    IEnumerator resetPlayerAndBall(float secs)
    {
        yield return new WaitForSeconds(secs);
        resetPosition = true;
    }

    public void updatePlayerBallTransform()
    {
        player.transform.position = playerStartPosition;
        player.transform.rotation = playerStartRotation;

        ball.transform.position = ballStartPosition;
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        ballWasPickedUp = false;
        waitInitiated = false;
        resetPosition = false;
    }
}
