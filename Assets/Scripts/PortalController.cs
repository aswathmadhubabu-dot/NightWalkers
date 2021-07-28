using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PortalController : MonoBehaviour
{
    public float nextLevel;
    public Text nextLevelText;
    private PlayerControlScript playerControlScript;

    void OnTriggerEnter(Collider other)
    {
        Destroy(GameObject.Find("TimerUI"));
        playerControlScript.EnablePlayer(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        //TODO: Stop actual game time
        //TODO: Play dance animation
        Time.timeScale = 0f;
        switch (nextLevel)
        {
            case 1:
                GameObject dm = GameObject.Find("NextLevelMenuContainer").transform.GetChild(0).gameObject;
                dm.SetActive(true);
                break;
            case 2:
                GameObject dm2 = GameObject.Find("NextLevelMenuContainer").transform.GetChild(0).gameObject;
                dm2.SetActive(true);
                break;
            case 3:
                GameObject dm3 = GameObject.Find("NextLevelMenuContainer").transform.GetChild(0).gameObject;
                dm3.SetActive(true);
                break;
            default:
                GameObject gameObject = GameObject.Find("YouWonMenuContainer").transform.GetChild(0).gameObject;
                gameObject.SetActive(true);
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
        switch (nextLevel)
        {
            case 1:
                Time.timeScale = 1f;
                SceneManager.LoadScene("SimpleLevel");
                break;
            case 2:
                Time.timeScale = 1f;
                SceneManager.LoadScene("MediumLevel");
                break;
            case 3:
                Time.timeScale = 1f;
                SceneManager.LoadScene("MazeLevel");
                break;
            default:
                Time.timeScale = 1f;
                SceneManager.LoadScene("GameMenu");
                break;
        }
    }
}