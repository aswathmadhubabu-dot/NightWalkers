using System.Collections.Generic;
using UnityEngine;

public class TeamScript : MonoBehaviour
{
    public int score;
    public string teamName;


    public MatchController match;

    public List<GameObject> players;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        match = GameObject.Find("Match").GetComponent<MatchController>();
    }

    public void ScoreGoal()
    {
        score += 1;
        match.NewGoal(this);
    }
}