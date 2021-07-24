using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerController : MonoBehaviour
{
    public GameObject player;
    public ParticleSystem flame;


    void Start(){
        //explosion = GetComponent<>();
    }

    void OnTriggerEnter(Collider other){
        Debug.Log("Walked on FlameThrower!");
        if(other.gameObject == player){
            //Explode
            //TODO less damage if you are FAR
            player.GetComponent<HealthController>().TakeDamage(10f, this.gameObject);
            //GameObject particle = Instantiate(explosiveParticle, transform.position, Quaternion.identity); 
            flame.Play(true);
            flame.Emit(35);
        }
    }
}
