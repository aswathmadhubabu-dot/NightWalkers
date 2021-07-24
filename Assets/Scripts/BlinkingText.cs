using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingText : MonoBehaviour
{
    public Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    IEnumerator Blink()
    {
        while (true)
        {
            switch (text.color.a.ToString())
            {
                case "0":
                    text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
                    yield return new WaitForSeconds(0.5f);
                    break;
                case "1":
                    text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
                    yield return new WaitForSeconds(0.5f);
                    break;
            }
        }
    }

    public void StartBlinking()
    {
        StopCoroutine("Blink");
        StartCoroutine("Blink");
    }

    public void StopBlinking()
    {
        StopCoroutine("Blink");
    }

    public void showText(String msg)
    {
        text.text = msg;
    }
}