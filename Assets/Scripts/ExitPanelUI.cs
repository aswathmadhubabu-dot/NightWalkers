using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPanelUI : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] public MessageUI scoreBoard;


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void setScoreMessage(String msg)
    {
        scoreBoard.showMessage(msg);
    }
}