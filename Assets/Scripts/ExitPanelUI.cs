using System;
using UnityEngine;

public class ExitPanelUI : MonoBehaviour
{
    [SerializeField] public MessageUI scoreBoard;

    public void setScoreMessage(String msg) => scoreBoard.showMessage(msg);
}