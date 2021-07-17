using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MatchController : MonoBehaviour
{
    public TextMeshProUGUI goalText;
    public TextMeshProUGUI timeText;
    public int gameTimeInSecs;
    private float remainingTime;
    public GameObject ball;
    public GameObject ballSpawn;
    public TeamScript teamA;

    public TeamScript teamB;
    public bool recentGoal;
    public int goalTimeout;

    void Start()
    {
        remainingTime = gameTimeInSecs;
        goalText.enabled = false;
        recentGoal = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }

        float minutes = Mathf.FloorToInt(remainingTime / 60);
        float seconds = Mathf.FloorToInt(remainingTime % 60);
        timeText.text = minutes.ToString() + " : " + seconds.ToString();
    }

    public void NewGoal(TeamScript happyTeam)
    {
        goalText.text = "" + happyTeam.teamName + " Goal!";
        StartCoroutine(ResetPlayersAndBall(happyTeam));
    }

    IEnumerator ResetPlayersAndBall(TeamScript happyTeam)
    {
        recentGoal = true;
        goalText.enabled = true;
        //TeamScript angryTeam = (teamA.name == happyTeam.name) ? teamB : teamA;

        //setAngry(angryTeam);
        //setHappy(happyTeam);


        yield return new WaitForSeconds(goalTimeout);

        //ball.transform.position = ballSpawn.transform.position;
        //ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        //resetPlayerPos(teamA);
        //resetPlayerPos(teamB);
        //setNormal(teamA);
        //setNormal(teamB);

        goalText.enabled = false;
        recentGoal = false;
    }

    void resetPlayerPos(TeamScript team)
    {
        foreach (GameObject player in team.players)
        {
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            player.transform.position = player.GetComponent<PlayerController>().originalPosition.position;
            player.transform.rotation = player.GetComponent<PlayerController>().originalPosition.rotation;
        }
    }

    void setHappy(TeamScript team)
    {
        foreach (GameObject player in team.players)
        {
            player.GetComponent<Animator>().SetInteger("Emotion", 1);
        }
    }

    void setAngry(TeamScript team)
    {
        foreach (GameObject player in team.players)
        {
            player.GetComponent<Animator>().SetInteger("Emotion", 2);
        }
    }

    void setNormal(TeamScript team)
    {
        foreach (GameObject player in team.players)
        {
            player.GetComponent<Animator>().SetInteger("Emotion", 0);
        }
    }
}