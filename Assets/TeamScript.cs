using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TeamScript : MonoBehaviour
{
    public int score;
    public TextMeshProUGUI scoreBoardText;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        scoreBoardText.text = score.ToString();

    }

}
