using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;
using Cinemachine;
public class HealthController : MonoBehaviour
{
    float currentHealth;
    public float maxHealth = 100f;
    Ragdoll ragdoll;

    public  event Action<float> OnHealthPctChanged = delegate{};

    public float blinkIntensity;
    public float blinkDuration;
    float blinkTimer;

    CameraManager cameraManager;
    SkinnedMeshRenderer[] skinnedMeshRenderer;

    VolumeProfile postProcessing;
    public void TakeDamage(float damage, GameObject source){
        Vignette vignette;
        currentHealth -= damage;
        if(currentHealth <= 0){
            currentHealth = 0;
            Die(source);
        }
        float currentHealthPct = (float) currentHealth / (float) maxHealth;
        if(postProcessing.TryGet(out vignette)){
            vignette.intensity.value = (1 - currentHealthPct) * 0.5f;
        }

        blinkTimer = blinkDuration;
        OnHealthPctChanged(currentHealthPct);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        ragdoll = GetComponent<Ragdoll>();
        skinnedMeshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        cameraManager = FindObjectOfType<CameraManager>();
        postProcessing = FindObjectOfType<Volume>().profile;
    }

    // Update is called once per frame
    void Die(GameObject source)
    {
        ragdoll.ActivateRagdoll();
        CinemachineTargetGroup tg = cameraManager.GetComponentInChildren<CinemachineTargetGroup>();
        Cinemachine.CinemachineTargetGroup.Target target;
        target.target = source.transform;
        target.weight = 0.5f;
        target.radius = 2f;
        Cinemachine.CinemachineTargetGroup.Target player;
        //Dont use the player transform because charactercontroller was disabled to enable ragdoll;
        player.target = GameObject.Find("mixamorig1:Hips").transform;
        player.weight = 1f;
        player.radius = 2f;
        tg.m_Targets =  new CinemachineTargetGroup.Target[2];
        tg.m_Targets[0] = player;
        tg.m_Targets[1] = target;
        cameraManager.EnableKillCam();
        GameObject dm = GameObject.Find("DeathMenuContainer").transform.GetChild(0).gameObject;
        dm.SetActive(true);

    }

    void Update(){
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01( blinkTimer / blinkDuration);
        float intensity = (lerp * blinkIntensity) + 1.0f;
        foreach( var smr in skinnedMeshRenderer){
            foreach( var mat in smr.materials){
                mat.color = Color.white * intensity;
            }
        }
    }
}
