using System.Collections;
using UnityEngine;

public class MatchController : MonoBehaviour
{
    public int gameTimeInSecs;
    public bool recentGoal;
    public int goalTimeout;

    [SerializeField] public Timer timer;

    [SerializeField] public MessageUI goalMessage;

    [SerializeField] public PlayerControlScript playerControlScript;

    [SerializeField] public ExitPanelUI exitPanel;

    [SerializeField] public MessageUI scoreBoard;

    private int goalsScored = 0;

    void Start()
    {
        setInitialGameParams();
    }

    void setInitialGameParams()
    {
        playerControlScript.EnablePlayer(true);
        goalsScored = 0;
        recentGoal = false;
        StartTimer(gameTimeInSecs);

        exitPanel.gameObject.SetActive(false);
        exitPanel.enabled = false;
    }

    void StartTimer(int duration)
    {
        if (timer != null)
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
                })
                .OnEnd(OnTimerEnded)
                .Begin();
        }
    }

    // Update is called once per frame
    void OnTimerEnded()
    {
        Debug.Log("Timer ended");

        playerControlScript.EnablePlayer(false);
        exitPanel.gameObject.SetActive(true);

        if (goalsScored == 1)
        {
            exitPanel.setScoreMessage(goalsScored + " Goal scored");
        }
        else
        {
            exitPanel.setScoreMessage(goalsScored + " Goals scored");
        }

        if (goalsScored > 0)
        {
            playerControlScript.MakePlayerDance();
        }
        else
        {
            playerControlScript.MakePlayerDefeated();
        }
    }

    public void NewGoal()
    {
        goalsScored += 1;
        goalMessage.toggleVisibility(true);
        StartCoroutine(ResetPlayersAndBall());
    }

    IEnumerator ResetPlayersAndBall()
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

    public void OnUserClickedExit()
    {
        Debug.Log("ON User Ext");
    }

    public void OnUserClickedRestart()
    {
        playerControlScript.
        StartCoroutine(ResetPlayersAndBall());
        setInitialGameParams();
    }
}