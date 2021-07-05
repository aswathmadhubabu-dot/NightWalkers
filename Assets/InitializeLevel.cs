using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeLevel : MonoBehaviour
{
    [SerializeField]
    private Transform[] teamOrangePlayerSpawns;
    [SerializeField]
    private Transform[] teamBluePlayerSpawns;

    [SerializeField]
    private GameObject playerPrefabs;
    public GameObject cam;

    // Start is called before the first frame update
    void Awake()
    {
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
        Debug.Log("Creating players: " + playerConfigs.Length);
        for( var i = 0; i < playerConfigs.Length; i++)
        {
            Debug.Log("Creating player: " + playerConfigs.Length);
            Transform spawn = GetSpawn(playerConfigs[i]);

            var player = Instantiate(playerPrefabs,  spawn.position, spawn.rotation, gameObject.transform);
            PlayerController pc = player.GetComponent<PlayerController>();
            pc.originalPosition = spawn;
            pc.InitializePlayer(playerConfigs[i]);
            Debug.Log("Initialized player: ");
            cam.GetComponent<CameraController>().targets.Add(player.transform);
            Debug.Log("Added player to Camera: ");
            GameObject.Find("Team " + playerConfigs[i].team).GetComponent<TeamScript>().players.Add(player);
            Debug.Log("Added player to team: " + playerConfigs.Length);

        }
    }
    Transform GetSpawn(PlayerConfiguration pi)
    {
        if(pi.team == "Orange"){
            return teamOrangePlayerSpawns[pi.teamPlayerIndex];
        } else{
            return teamBluePlayerSpawns[pi.teamPlayerIndex];
        }
    }
}
