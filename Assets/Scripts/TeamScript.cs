using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TeamScript : MonoBehaviour
{
    public int score;
    public string teamName;

    [SerializeField] public MessageUI scoreBoard;

    public MatchController match;

    public List<GameObject> players;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        match = GameObject.Find("Match").GetComponent<MatchController>();
        scoreBoard.toggleVisibility(true);
    }

    public void ScoreGoal()
    {
        score += 1;
        match.NewGoal(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (score == 1)
        {
            scoreBoard.showMessage(score + " Goal scored");
        }
        else
        {
            scoreBoard.showMessage(score + " Goals scored");
        }
    }
}