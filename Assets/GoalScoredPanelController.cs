using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class GoalScoredPanelController : MonoBehaviour
{
    [SerializeField] public Button continueButton;
    [SerializeField] public TextMeshProUGUI goalText;

    public CanvasGroup canvasGroup;

    public void show(bool shouldShow, String message)
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
            goalText.text = message;
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