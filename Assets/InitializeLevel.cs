using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeLevel : MonoBehaviour
{
    [SerializeField]
    private Transform[] teamOrangePlayerSpawns;
    [SerializeField]
    private Transform[] teamBluePlayerSpawns;
    public GameObject ball;
    public GameObject goal_blue;
    public GameObject goal_orange;

    [SerializeField]
    private GameObject playerPrefabs;
    public GameObject cam;
    public GameObject AIplayerPrefabs;
    int availableSlotsOnBlue = 2;
    int availableSlotsOnOrange = 2;

    public Material orangeMaterial;
    public Material blueMaterial;

    // Start is called before the first frame update
    void Awake()
    {
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
        Debug.Log("Creating players: " + playerConfigs.Length);
        for (var i = 0; i < playerConfigs.Length; i++)
        {
            Debug.Log("Creating player: " + playerConfigs.Length);
            Transform spawn = GetSpawn(playerConfigs[i]);

            var player = Instantiate(playerPrefabs, spawn.position, spawn.rotation, gameObject.transform);
            PlayerController pc = player.GetComponent<PlayerController>();
            pc.originalPosition = spawn;
            pc.InitializePlayer(playerConfigs[i]);
            Debug.Log("Initialized player: ");
            cam.GetComponent<CameraController>().targets.Add(player.transform);
            Debug.Log("Added player to Camera: ");
            GameObject.Find("Team " + playerConfigs[i].team).GetComponent<TeamScript>().players.Add(player);
            Debug.Log("Added player to team: " + playerConfigs.Length);

        }

        for (var i = 0; i < availableSlotsOnBlue; i++)
        {
            Transform spawn = GetSpawn("Blue", i);
            var AIplayer = Instantiate(AIplayerPrefabs, spawn.position, spawn.rotation, gameObject.transform);
            PlayersAIScript pai = AIplayer.GetComponent<PlayersAIScript>();
            pai.InitializeAI(ball, goal_orange, "Team1", blueMaterial);
            cam.GetComponent<CameraController>().targets.Add(AIplayer.transform);
            GameObject.Find("Team Blue").GetComponent<TeamScript>().players.Add(AIplayer);
        }
        for (var i = 0; i < availableSlotsOnOrange; i++)
        {
            Transform spawn = GetSpawn("Orange", i);
            var AIplayer = Instantiate(AIplayerPrefabs, spawn.position, spawn.rotation, gameObject.transform);
            PlayersAIScript pai = AIplayer.GetComponent<PlayersAIScript>();
            pai.InitializeAI(ball, goal_blue, "Team2", orangeMaterial);
            cam.GetComponent<CameraController>().targets.Add(AIplayer.transform);
            GameObject.Find("Team Orange").GetComponent<TeamScript>().players.Add(AIplayer);
        }
    }
    Transform GetSpawn(PlayerConfiguration pi)
 

    {
        if (pi.team == "Orange")
        {
            availableSlotsOnOrange--;
            return teamOrangePlayerSpawns[pi.teamPlayerIndex];
        }
        else
        {
            availableSlotsOnBlue--;
            return teamBluePlayerSpawns[pi.teamPlayerIndex];
        }
    }

    Transform GetSpawn(string team, int pi)
    {
        if(team == "Orange"){
            return teamOrangePlayerSpawns[1 - pi];
        } else
        {
            return teamBluePlayerSpawns[1 - pi];
        }
    }

}
