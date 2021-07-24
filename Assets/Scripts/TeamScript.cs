using System.Collections.Generic;
using UnityEngine;

public class TeamScript : MonoBehaviour
{
    public int score;
    public string teamName;


    public MatchController match;

    public List<GameObject> players;

    // Start is called before the first frame update
    void Start() => match = GameObject.Find("Match").GetComponent<MatchController>();
}