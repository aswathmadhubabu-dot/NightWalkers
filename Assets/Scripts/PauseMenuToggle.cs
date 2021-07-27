using UnityEngine;
using UnityEngine.SceneManagement;

// [RequireComponent(typeof(CanvasGroup))]
public class PauseMenuToggle : MonoBehaviour
{
    // private CanvasGroup canvasGroup;
    private static bool _isGamePaused = false;

    public GameObject pauseMenuUI;
    //
    // private void Awake()
    // {
    //     canvasGroup = GetComponent<CanvasGroup>();
    //     if (canvasGroup == null)
    //     {
    //         Debug.LogError("Unable to pick canvas group");
    //     }
    // }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            // if (canvasGroup.interactable)
            // {
            //     canvasGroup.interactable = false;
            //     canvasGroup.blocksRaycasts = false;
            //     canvasGroup.alpha = 0f;
            //     Time.timeScale = 1f;
            //     // SceneManager.UnloadSceneAsync("Minigame");
            // }
            // else
            // {
            //     canvasGroup.alpha = 1f;
            //     canvasGroup.interactable = true;
            //     canvasGroup.blocksRaycasts = true;
            //     Time.timeScale = 0f;
            //     // SceneManager.LoadScene("Minigame", LoadSceneMode.Additive);
            // }
            if (_isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void LoadGame()
    {
        Time.timeScale = 1f;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        _isGamePaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        _isGamePaused = true;
    }
}