using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    public void StartGame() => SceneManager.LoadScene("Minigame");

    public void LoadGame()
    {
        SceneManager.LoadScene("Minigame");
        Time.timeScale = 1f;
    }
}