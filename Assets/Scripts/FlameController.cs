using UnityEngine;
using System.Collections;
public class FlameController : MonoBehaviour
{
    public GameObject flamesPrefab;

    private bool dealDamage = false;
    private bool stillColliding = false;

    // Start is called before the first frame update
    private HealthController objectBurning;
    public GameObject flames = null;
    public Transform flameOrigin;

    void OnTriggerEnter(Collider other)
    {
        if (!dealDamage && other.gameObject == GameObject.Find("AstraHumanoid"))
        {
            Debug.Log("START BURNING");
            objectBurning = other.gameObject.GetComponent<HealthController>();
            stillColliding = true;
            if(flames == null){
                StartCoroutine(WaitForDamage());
            } else {
                dealDamage = true;
            }
            flames = Instantiate(flamesPrefab, flameOrigin.position,
                this.transform.rotation);
        }
    }
    
    IEnumerator WaitForDamage(){
        yield return new WaitForSeconds(0.4f);
        if(stillColliding){
            dealDamage = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameObject.Find("AstraHumanoid"))
        {
            Debug.Log("STOP BURNING");
            dealDamage = false;
            stillColliding = false;
        }
    }

    void FixedUpdate()
    {
        if (dealDamage && objectBurning != null && flames != null)
        {
            objectBurning.TakeDamage(1f, this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}