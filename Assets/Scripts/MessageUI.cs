using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageUI : MonoBehaviour
{
    [SerializeField] private Text uiText;

    [SerializeField] private Image bgImage;
    [SerializeField] private Image fgImage;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void showMessage(String message)
    {
        uiText.text = message;
    }

    public void toggleVisibility(Boolean show)
    {
        bgImage.gameObject.SetActive(show);
        fgImage.gameObject.SetActive(show);
        uiText.gameObject.SetActive(show);

        bgImage.enabled = show;
        fgImage.enabled = show;
        uiText.enabled = show;
    }
}