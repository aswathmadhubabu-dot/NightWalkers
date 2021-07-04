using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Linq;

public class PlayerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigs;
    [SerializeField]
    private int MaxPlayers = 6;

    public static PlayerConfigurationManager Instance {get; private set;}
    void Awake(){
        if(Instance != null)
        {
            Debug.Log("SINGLETON - Trying to create another instance of singleton!!");
            
        } else {
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfiguration>();
            //Debug.Log("CREATING INITIAL!!");
        }
    }
    public List<PlayerConfiguration> GetPlayerConfigs(){
        return playerConfigs;
    }
    public void SetPlayerColor(int index, Material color)
    {
        playerConfigs[index].PlayerMaterial = color;
    } 
    public void ReadyPlayer(int index)
    {
        playerConfigs[index].IsReady = true;
        if(playerConfigs.Count >= 1 && playerConfigs.All(p => p.IsReady))
        {
            SceneManager.LoadScene("Minigame");
        }
    }
    public void HandlePlayerJoin(PlayerInput pi)
    {
        Debug.Log("Player joined " + pi.playerIndex);
        Debug.Log("Total Players" + (playerConfigs.Count + 1 ).ToString());

        if(!playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex))
        {

            pi.transform.SetParent(transform);
            playerConfigs.Add(new PlayerConfiguration(pi));

        }
    }

}
public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }
    public PlayerInput Input { get; private set;}
    public int PlayerIndex { get; private set; }
    
    public bool IsReady {get; set;}

    public Material PlayerMaterial {get; set;}
}