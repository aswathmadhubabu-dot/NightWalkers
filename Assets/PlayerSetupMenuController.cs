using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerSetupMenuController : MonoBehaviour
{
    private int PlayerIndex;
    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    GameObject readyPanel;
    [SerializeField]
    GameObject menuPanel;

    [SerializeField]
    private Button readyButton;
    
    private float ignoreInputTime = 1.5f;
    private bool inputEnabled;

    public void SetPlayerIndex(int pi)
    {
        PlayerIndex = pi;
        titleText.SetText("Player " + (pi + 1).ToString());
        ignoreInputTime = Time.time + ignoreInputTime;
    }
    void Update(){
        if(Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }

    }

    public void SetColor(Material color){
        if(!enabled){
            return;
        }
        PlayerConfigurationManager.Instance.SetPlayerColor(PlayerIndex, color);
        readyPanel.SetActive(true);
        readyButton.Select();
        menuPanel.SetActive(false);
    }

    public void ReadyPlayer()
    {
        if(!inputEnabled){return;}
        PlayerConfigurationManager.Instance.ReadyPlayer(PlayerIndex);
        readyButton.gameObject.SetActive(false);
    }
}
