using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void FirstLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SimpleLevel");
    }

    public void SecondLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MediumLevel");
    }

    public void ThirdLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MazeLevel");
    }
}