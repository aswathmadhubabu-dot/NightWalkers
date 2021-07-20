using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
public class HealthController : MonoBehaviour
{
    float currentHealth;
    public float maxHealth = 30;
    Ragdoll ragdoll;

    public  event Action<float> OnHealthPctChanged = delegate{};

    public float blinkIntensity;
    public float blinkDuration;
    float blinkTimer;
    SkinnedMeshRenderer[] skinnedMeshRenderer;
    public void TakeDamage(float damage){
        currentHealth -= damage;
        if(currentHealth <= 0){
            Die();
        }
        blinkTimer = blinkDuration;
        float currentHealthPct = (float) currentHealth / (float) maxHealth;
        OnHealthPctChanged(currentHealthPct);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        ragdoll = GetComponent<Ragdoll>();
        skinnedMeshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Die()
    {
        ragdoll.ActivateRagdoll();
    }

    void Update(){
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01( blinkTimer / blinkDuration);
        float intensity = (lerp * blinkIntensity) + 1.0f;
        foreach( var smr in skinnedMeshRenderer){
            smr.material.color = Color.white * intensity;
        }
    }
}
