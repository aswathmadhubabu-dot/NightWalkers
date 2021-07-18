using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class MatchController : MonoBehaviour
{
    public int gameTimeInSecs;
    public GameObject ball;
    public GameObject ballSpawn;
    public TeamScript teamA;

    public TeamScript teamB;
    public bool recentGoal;
    public int goalTimeout;

    [SerializeField] public Timer timer;

    [SerializeField] public MessageUI goalMessage;

    [SerializeField] public PlayerControlScript playerControlScript;

    void Start()
    {
        recentGoal = false;
        StartTimer(gameTimeInSecs);
        goalMessage.toggleVisibility(false);
    }

    void StartTimer(int duration)
    {
        timer
            .SetDuration(duration)
            .OnBegin(() => Debug.Log("Timer began"))
            .OnChange(progress =>
            {
                if (progress < 20)
                {
                    timer.toggleBlinking(true);
                }

                Debug.Log("On Timer change");
            })
            .OnEnd(OnTimerEnded)
            .Begin();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void endCurrentLevel()
    {
        if (teamA.score > 0)
        {
            playerControlScript.MakePlayerDance();
        }
        else
        {
            playerControlScript.MakePlayerDefeated();
        }
    }

    void OnTimerEnded()
    {
        endCurrentLevel();
        Debug.Log("Timer ended");
    }

    public void NewGoal(TeamScript happyTeam)
    {
        goalMessage.toggleVisibility(true);
        StartCoroutine(ResetPlayersAndBall(happyTeam));
    }

    IEnumerator ResetPlayersAndBall(TeamScript happyTeam)
    {
        recentGoal = true;
        //TeamScript angryTeam = (teamA.name == happyTeam.name) ? teamB : teamA;

        //setAngry(angryTeam);
        //setHappy(happyTeam);


        yield return new WaitForSeconds(goalTimeout);
        goalMessage.toggleVisibility(false);

        //ball.transform.position = ballSpawn.transform.position;
        //ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        //resetPlayerPos(teamA);
        //resetPlayerPos(teamB);
        //setNormal(teamA);
        //setNormal(teamB);

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