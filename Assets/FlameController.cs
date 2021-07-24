using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameController : MonoBehaviour
{
    public GameObject flamesPrefab;
    private bool dealDamage = false;
    // Start is called before the first frame update
    private HealthController objectBurning;
    GameObject flames;
    void OnTriggerEnter(Collider other){
        if(!dealDamage && other.gameObject == GameObject.Find("AstraHumanoid")){
            Debug.Log("START BURNING");
            objectBurning = other.gameObject.GetComponent<HealthController>();
            dealDamage = true;
            flames = Instantiate(flamesPrefab, this.transform.position - new Vector3( 0, 0, 3f), this.transform.rotation);
        }
    }

    void OnTriggerExit(Collider other){
        if(other.gameObject == GameObject.Find("AstraHumanoid")){
            Debug.Log("STOP BURNING");
            dealDamage = false;
        }
    }
    void FixedUpdate()
    {
        if(dealDamage && objectBurning != null && flames != null){
            objectBurning.TakeDamage(1f, this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
