using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerSetupMenuController : MonoBehaviour
{
    private int PlayerIndex;
    [SerializeField]
    private TextMeshProUGUI titleText;

    [SerializeField]
    public TextMeshProUGUI readyText;
    [SerializeField]
    GameObject readyPanel;
    [SerializeField]
    GameObject menuPanel;

    [SerializeField]
    private Button readyButton;
    
    private float ignoreInputTime = 1.5f;
    private bool inputEnabled;

    private string color;
    private string team;
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
        if(!inputEnabled){
            return;
        }
        
        if(color.name == "LilRobot Red"){
           this.color = "Red";
           team = "Orange";
        }
        if(color.name == "LilRobot Orange"){
           this.color = "Orange";
           team = "Orange";
        }
        if(color.name == "LilRobot LightBlue"){
           this.color = "LightBlue";
           team = "Blue";
        }
        if(color.name == "LilRobot Blue"){
           this.color = "Blue";
           team = "Blue";
        }
        PlayerConfigurationManager.Instance.SetPlayerColor(PlayerIndex, color, team);
        readyPanel.SetActive(true);
        readyButton.Select();
        menuPanel.SetActive(false);
    }

    public void ReadyPlayer()
    {
        if(!inputEnabled){return;}
        PlayerConfigurationManager.Instance.ReadyPlayer(PlayerIndex);
        readyButton.gameObject.SetActive(false);
        readyText.text = "<b>Ready!</b>\n Team " + team + "\nColor: " + color;

    }
}
