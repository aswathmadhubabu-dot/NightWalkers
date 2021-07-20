using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image foregroundImage;
    private float updateSpeedSeconds = 0.5f;

    // Start is called before the first frame update
    private void Awake()
    {
        GetComponentInParent<HealthController>().OnHealthPctChanged += HandleHealthChanged;
    }

    private IEnumerator ChangeToPct(float pct){
        float preChangePct = foregroundImage.fillAmount;
        float elapsed = 0f;
        while (elapsed < updateSpeedSeconds){
            elapsed += Time.deltaTime;
            foregroundImage.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds );
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
