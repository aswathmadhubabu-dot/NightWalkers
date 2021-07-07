using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateUpdateScript : MonoBehaviour
{
    private GameObject[] team1Members;
    private GameObject[] team2Members;
    public GameObject ball;
    // Start is called before the first frame update
    void Start()
    {
        team1Members = GameObject.FindGameObjectsWithTag("Team1");
        team2Members = GameObject.FindGameObjectsWithTag("Team2");
        updateAIStates(team1Members);
        updateAIStates(team2Members);
    }

    // Update is called once per frame
    void Update()
    {
        updateAIStates(team1Members);
        updateAIStates(team2Members);
    }

    private void updateAIStates(GameObject[] members)
    {
        float minDistance = 10000f;
        GameObject minPlayer = null;

        foreach (GameObject member in members)
        {
            float distance = (member.transform.position - ball.transform.position).magnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                minPlayer = member;
            }
        }

        if (minPlayer != null)
        {
            PlayersAIScript minPlayerScript = minPlayer.GetComponent<PlayersAIScript>();
            if (minPlayerScript != null)
            {
                //Debug.Log("SETTING ATTACKING");
                minPlayerScript.playerState = PlayerState.Attacking;
            }
            foreach (GameObject member in members)
            {
                if (member.name != minPlayer.name)
                {
                    PlayersAIScript memberPlayerScript = member.GetComponent<PlayersAIScript>();
                    if (memberPlayerScript)
                    {
                        memberPlayerScript.playerState = PlayerState.Idle;
                    }
                }
            }
        }
    }
}
