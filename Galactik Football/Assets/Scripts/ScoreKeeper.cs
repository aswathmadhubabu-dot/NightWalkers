using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    public int score = 0;
    public Text text;


    void Start() => text.text = score.ToString();

    public void ScoreGoal()
    {
        this.score++;
        text.text = this.score.ToString();
    }
}
