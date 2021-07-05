using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class TimeEndedPanelController : MonoBehaviour
{
    [SerializeField] public Button restartButton;

    [SerializeField] public TextMeshProUGUI teamAScore;
    [SerializeField] public TextMeshProUGUI teamBScore;

    public CanvasGroup canvasGroup;

    public void show(bool shouldShow, TeamScript teamAScript, TeamScript teamBScript)
    {
        if (!shouldShow)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0f;
            Time.timeScale = 1f;
        }
        else
        {
            teamAScore.text = teamAScript.score.ToString();
            teamBScore.text = teamBScript.score.ToString();

            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
            Time.timeScale = 0f;
        }
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
}