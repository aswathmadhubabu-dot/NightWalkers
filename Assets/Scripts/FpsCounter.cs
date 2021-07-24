using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class FpsCounter : MonoBehaviour
{

    [FormerlySerializedAs("_fpsText")] [SerializeField] public TextMeshProUGUI fpsText;
    [FormerlySerializedAs("_hudRefreshRate")] [SerializeField] private float hudRefreshRate = 1f;
 
    private float timer;
 
    private void Update()
    {
        if (Time.unscaledTime > timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            fpsText.text = "FPS: " + fps;
            timer = Time.unscaledTime + hudRefreshRate;
        }
    }
}
