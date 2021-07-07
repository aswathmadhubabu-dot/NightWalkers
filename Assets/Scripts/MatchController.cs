using System.Collections;
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

    private TeamScript lastScoredTeam;

    [SerializeField] private Timer timer;
    [SerializeField] private GoalScoredPanelController goalScoredPanelController;
    [SerializeField] private TimeEndedPanelController timeEndedPanelController;

    void Start()
    {
        recentGoal = false;

        timer
            .SetDuration(gameTimeInSecs)
            .OnEnd(() =>
            {
                Debug.Log("Timer ended");
                DisplayTimeEnded();
            })
            .Begin();

        goalScoredPanelController.show(false, "");
        timeEndedPanelController.show(false, teamA, teamB);
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

    private void RestartClickedOnTimeEnded()
    {
        // TODO: Redirect to landing screen
    }

    void DisplayTimeEnded()
    {
        timeEndedPanelController.restartButton.onClick.AddListener(RestartClickedOnTimeEnded);
        timeEndedPanelController.show(true, teamA, teamB);
    }

    public void NewGoal(TeamScript happyTeam)
    {
        lastScoredTeam = happyTeam;
        goalScoredPanelController.continueButton.onClick.AddListener(ContinueClickedOnGoalScore);
        goalScoredPanelController.show(true, "" + happyTeam.teamName + " has scored a Goal!");
    }

    IEnumerator ResetPlayersAndBall(TeamScript happyTeam)
    {
        recentGoal = true;
        TeamScript angryTeam = (teamA.name == happyTeam.name) ? teamB : teamA;

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
            if(player.GetComponent<PlayerController>() != null){
                player.transform.position = player.GetComponent<PlayerController>().originalPosition.position;
                player.transform.rotation = player.GetComponent<PlayerController>().originalPosition.rotation;
            } else {
                    //TODO RESTART AIs
            }
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