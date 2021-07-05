using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MatchController : MonoBehaviour
{
    public int gameTimeInSecs;

    public GameObject ball;
    public GameObject ballSpawn;

    public TeamScript teamA;
    public TeamScript teamB;

    public bool recentGoal;
    public int goalTimeout;

    public TeamScript lastScoredTeam;
    
    [SerializeField] private Timer timer;
    [SerializeField] private GoalScoredPanelController goalScoredPanelController;

    void Start()
    {
        recentGoal = false;

        timer
            .SetDuration(gameTimeInSecs)
            .OnEnd(() => Debug.Log("Timer 1 ended"))
            .Begin();

        goalScoredPanelController.show(false, "");
    }

    private void ContinueClickedOnGoalScore()
    {
        goalScoredPanelController.show(false, "");
        StartCoroutine(ResetPlayersAndBall(lastScoredTeam));
    }

    // Update is called once per frame
    void Update()
    {
    }

    void DisplayTimeEnded()
    {
    }

    public void NewGoal(TeamScript happyTeam)
    {
        // goalText.text = "" + happyTeam.teamName + " Goal!";
        lastScoredTeam = happyTeam;
        goalScoredPanelController.continueButton.onClick.AddListener(ContinueClickedOnGoalScore);
        goalScoredPanelController.show(true, "" + happyTeam.teamName + " has scored a Goal!");
    }

    IEnumerator ResetPlayersAndBall(TeamScript happyTeam)
    {
        recentGoal = true;
        TeamScript angryTeam;
        if (teamA.name == happyTeam.name)
        {
            angryTeam = teamB;
        }
        else
        {
            angryTeam = teamA;
        }

        setAngry(angryTeam);
        setHappy(happyTeam);


        yield return new WaitForSeconds(goalTimeout);

        ball.transform.position = ballSpawn.transform.position;
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        resetPlayerPos(teamA);
        resetPlayerPos(teamB);
        setNormal(teamA);
        setNormal(teamB);

        recentGoal = false;
    }

    void resetPlayerPos(TeamScript team)
    {
        foreach (GameObject player in team.players)
        {
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            player.transform.position = team.playerSpawn.transform.position;
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