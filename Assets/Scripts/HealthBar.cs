using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image foregroundImage;
    public Text healthText;
    public float maxHealth = 100f;
    private float updateSpeedSeconds = 0.5f;

    // Start is called before the first frame update
    private void Awake()
    {
        GetComponentInParent<HealthController>().OnHealthPctChanged += HandleHealthChanged;
        healthText.text = maxHealth.ToString() + " / " + maxHealth.ToString();
    }

    private IEnumerator ChangeToPct(float pct)
    {
        Debug.Log("New health %" + pct);
        float preChangePct = foregroundImage.fillAmount;
        float elapsed = 0f;
        healthText.text = (pct * maxHealth).ToString() + "/" + maxHealth.ToString();
        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            foregroundImage.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
            yield return null;
        }
    }

    private void LateUpdate()
    {
        //Rotation if needed
    }

    void HandleHealthChanged(float pct)
    {
        StartCoroutine(ChangeToPct(pct));
    }

    // Update is called once per frame
    void Update()
    {
    }
}