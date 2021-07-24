using UnityEngine;
using UnityEngine.SceneManagement;

public class MazeLevelGameStarter : MonoBehaviour
{
    public void StartGame() => SceneManager.LoadScene("MazeLevel");

    public void LoadGame()
    {
        SceneManager.LoadScene("MazeLevel");
        Time.timeScale = 1f;
    }
}