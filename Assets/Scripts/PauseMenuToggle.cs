using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasGroup))]
public class PauseMenuToggle : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public static bool isGamePaused = false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("Unable to pick canvas group");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (canvasGroup.interactable)
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                canvasGroup.alpha = 0f;
                Time.timeScale = 1f;
                // SceneManager.UnloadSceneAsync("Minigame");
            }
            else
            {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                Time.timeScale = 0f;
                // SceneManager.LoadScene("Minigame", LoadSceneMode.Additive);
            }
        }
    }
    
    public void LoadGame()
    {
        SceneManager.LoadScene("Minigame");
        Time.timeScale = 1f;
    }
}