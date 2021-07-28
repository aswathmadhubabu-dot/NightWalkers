using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PortalController : MonoBehaviour
{
    public float level;
    public Text nextLevelText;
    private PlayerControlScript playerControlScript;

    void OnTriggerEnter(){
        Destroy(GameObject.Find("TimerUI"));
        playerControlScript.EnablePlayer(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        //TODO: Stop actual game time
        //TODO: Play dance animation
        Time.timeScale = 0;
        switch(level){

            case 1:
                GameObject dm = GameObject.Find("NextLevelMenuContainer").transform.GetChild(0).gameObject;
                dm.SetActive(true);
            break;
            case 2:
                GameObject dm2 = GameObject.Find("NextLevelMenuContainer").transform.GetChild(0).gameObject;
                dm2.SetActive(true);
            break;
            case 3:
                GameObject dm3 = GameObject.Find("YouWonMenuContainer").transform.GetChild(0).gameObject;
                dm3.SetActive(true);
            break;
            
        }

    }

    void Start()
    {
        Time.timeScale = 1f;
        playerControlScript = FindObjectOfType<PlayerControlScript>();
    }

    public void LoadNextLevel()
    {
        nextLevelText.text = "Loading Level...";
        if (level == 1)
        {
            SceneManager.LoadScene("SimpleLevel");
        } else if (level == 2)
        {
            SceneManager.LoadScene("MazeLevel");
        }
    }
}
