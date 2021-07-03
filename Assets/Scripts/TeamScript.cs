using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TeamScript : MonoBehaviour
{
    public int score;
    public string teamName;
    public TextMeshProUGUI scoreBoardText;
    
    public GameObject playerSpawn;

    public MatchController match;

    public GameObject[] players;
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
    // Update is called once per frame
    void Update()
    {
        scoreBoardText.text = score.ToString();

    }

}
