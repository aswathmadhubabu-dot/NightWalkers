using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineController : MonoBehaviour
{
    public GameObject player;
    public ParticleSystem explosion;
    private bool enabled = true;

    void Start(){
        //explosion = GetComponent<>();
    }
    IEnumerator SelfDestruct(){
        yield return new WaitForSeconds(1.3f);
        this.gameObject.SetActive(false);
    }
    void OnTriggerEnter(Collider other){
        //Hack to prevent it from exploding TWICE
        if(!enabled){
            return;
        }
        if(other.gameObject == player){
            enabled = false;
            Debug.Log("Stepped on mine!");
            //Explode
            //TODO less damage if you are FAR
            player.GetComponent<HealthController>().TakeDamage(30f, this.gameObject);
            //GameObject particle = Instantiate(explosiveParticle, transform.position, Quaternion.identity); 
            explosion.Emit(20);
            StartCoroutine(SelfDestruct());
//            ParticleEffect pe = particle.GetComponent<ParticleEffect>();
//            pe.looping = false;
        }
    }
}
