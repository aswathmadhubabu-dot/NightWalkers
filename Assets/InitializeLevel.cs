using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeLevel : MonoBehaviour
{
    [SerializeField]
    private Transform[] playerSpawns;

    [SerializeField]
    private GameObject playerPrefabs;
    public GameObject cam;

    // Start is called before the first frame update
    void Start()
    {
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
        Debug.Log("Creating players: " + playerConfigs.Length);
        for( var i = 0; i >= playerConfigs.Length; i++)
        {
            var player = Instantiate(playerPrefabs, playerSpawns[i].position, playerSpawns[i].rotation, gameObject.transform);
            player.GetComponent<PlayerController>().InitializePlayer(playerConfigs[i]);
            cam.GetComponent<CameraController>().targets.Add(player.transform);
            GameObject.Find("Team A").GetComponent<TeamScript>().players.Add(player);

        }
    }

}
