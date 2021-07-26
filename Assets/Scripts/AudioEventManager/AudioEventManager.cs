using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioEventManager : MonoBehaviour
{

    public EventSound3D eventSound3DPrefab;

    public AudioClip hitGoalAudio;
    public AudioClip hitBallAudio;
    public AudioClip punchBallAudio;
    public AudioClip ovationAudio1;
    public AudioClip ovationAudio2;
    public AudioClip ovationAudio3;
    public AudioClip throwBallAudio;
    public AudioClip hitGlassAudio;
    public AudioClip shootAudio;
    public AudioClip aimAudio;

    private UnityAction<Vector3> hitGoalEventListener;

    private UnityAction<Vector3> throwBallEventListener;

    private UnityAction<Vector3> hitGlassEventListener;

    private UnityAction<Vector3> shootEventListener;
    private UnityAction<Vector3> aimEventListener;

    private UnityAction<Vector3, float> hitBallEventListener;

 
    private UnityAction<Vector3, float> punchBallEventListener;

    void Awake()
    {
        hitGlassEventListener = new UnityAction<Vector3>(hitGlassEventHandler);

        shootEventListener = new UnityAction<Vector3>(shootEventHandler);
        aimEventListener = new UnityAction<Vector3>(aimEventHandler);

        hitGoalEventListener = new UnityAction<Vector3>(hitGoalEventHandler);

        throwBallEventListener = new UnityAction<Vector3>(throwBallEventHandler);

        hitBallEventListener = new UnityAction<Vector3, float>(hitBallEventHandler);

        punchBallEventListener = new UnityAction<Vector3, float>(punchBallEventHandler);
    }


    // Use this for initialization
    void Start()
    {



    }


    void OnEnable()
    {
        EventManager.StartListening<AimEvent, Vector3>(aimEventListener);
        EventManager.StartListening<ShootEvent, Vector3>(shootEventListener); 
        EventManager.StartListening<HitGlassEvent, Vector3>(hitGlassEventListener);
        EventManager.StartListening<HitGoalEvent, Vector3>(hitGoalEventListener);
        EventManager.StartListening<ThrowBallEvent, Vector3>(throwBallEventListener);
        EventManager.StartListening<HitBallEvent, Vector3, float>(hitBallEventListener);
        EventManager.StartListening<PunchBallEvent, Vector3, float>(punchBallEventListener);
    }

    void OnDisable()
    {
        EventManager.StopListening<AimEvent, Vector3>(aimEventListener);
        EventManager.StopListening<ShootEvent, Vector3>(shootEventListener); 
        EventManager.StopListening<HitGlassEvent, Vector3>(hitGlassEventListener);
        EventManager.StopListening<HitGoalEvent, Vector3>(hitGoalEventListener);
        EventManager.StopListening<ThrowBallEvent, Vector3>(throwBallEventListener);
        EventManager.StopListening<HitBallEvent, Vector3, float>(hitBallEventListener);
        EventManager.StopListening<PunchBallEvent, Vector3, float>(punchBallEventListener);
    }




    void hitGoalEventHandler(Vector3 worldPos)
    {
        EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);
        EventSound3D snd2 = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);
        EventSound3D snd3 = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);
        EventSound3D snd4 = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);
        EventSound3D snd5 = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

        snd.audioSrc.clip = this.hitGoalAudio;

        snd.audioSrc.minDistance = 10f;
        snd.audioSrc.maxDistance = 500f;
        snd.audioSrc.volume = 0.4f;

        snd.audioSrc.Play();

        snd2.audioSrc.clip = this.ovationAudio1;

        snd2.audioSrc.minDistance = 10f;
        snd2.audioSrc.maxDistance = 500f;
        snd2.audioSrc.volume = 0.4f;

        snd2.audioSrc.PlayDelayed(0.5f);
        

        snd3.audioSrc.clip = this.ovationAudio2;

        snd3.audioSrc.minDistance = 10f;
        snd3.audioSrc.maxDistance = 500f;
        snd3.audioSrc.volume = 0.4f;

        snd3.audioSrc.PlayDelayed(0.5f);

        snd4.audioSrc.clip = this.ovationAudio2;

        snd4.audioSrc.minDistance = 10f;
        snd4.audioSrc.maxDistance = 500f;
        snd4.audioSrc.volume = 0.4f;

        snd4.audioSrc.PlayDelayed(0.1f);

        snd5.audioSrc.clip = this.ovationAudio3;
        
        snd5.audioSrc.minDistance = 10f;
        snd5.audioSrc.maxDistance = 500f;
        snd5.audioSrc.volume = 0.4f;
        snd5.audioSrc.PlayDelayed(0.1f);
    }

    void throwBallEventHandler(Vector3 worldPos)
    {
        //AudioSource.PlayClipAtPoint(this.boxAudio, worldPos);

        EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

        snd.audioSrc.clip = this.throwBallAudio;

        snd.audioSrc.minDistance = 10f;
        snd.audioSrc.maxDistance = 500f;

        snd.audioSrc.Play();
    }

    void hitGlassEventHandler(Vector3 worldPos)
    {
        //AudioSource.PlayClipAtPoint(this.boxAudio, worldPos);

        EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

        snd.audioSrc.clip = this.hitGlassAudio;

        snd.audioSrc.minDistance = 10f;
        snd.audioSrc.maxDistance = 500f;

        snd.audioSrc.Play();
    }

    void shootEventHandler(Vector3 worldPos)
    {
        //AudioSource.PlayClipAtPoint(this.boxAudio, worldPos);

        EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

        snd.audioSrc.clip = this.shootAudio;

        snd.audioSrc.minDistance = 10f;
        snd.audioSrc.maxDistance = 500f;

        snd.audioSrc.Play();
    }

    void aimEventHandler(Vector3 worldPos)
    {
        //AudioSource.PlayClipAtPoint(this.boxAudio, worldPos);

        EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

        snd.audioSrc.clip = this.aimAudio;

        snd.audioSrc.minDistance = 10f;
        snd.audioSrc.maxDistance = 500f;

        snd.audioSrc.Play();
    }

    void hitBallEventHandler(Vector3 worldPos, float impactForce)
    {
        //AudioSource.PlayClipAtPoint(this.boxAudio, worldPos);

        EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);
        
        snd.audioSrc.clip = this.hitBallAudio;

        snd.audioSrc.minDistance = Mathf.Lerp(1f, 8f, impactForce / 200f);
        snd.audioSrc.maxDistance = 500f;

        snd.audioSrc.Play();
    }

    void punchBallEventHandler(Vector3 worldPos, float impactForce)
    {
        //AudioSource.PlayClipAtPoint(this.boxAudio, worldPos);

        EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

        snd.audioSrc.clip = this.punchBallAudio;

        snd.audioSrc.minDistance = Mathf.Lerp(5f, 15f, impactForce / 200f);
        snd.audioSrc.maxDistance = 500f;

        snd.audioSrc.Play();
    }

}
