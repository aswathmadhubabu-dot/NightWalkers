using System.Collections;
using UnityEngine;

public class MineController : MonoBehaviour
{
    public GameObject player;
    public ParticleSystem explosion;
    private bool enabled = true;

    void Start()
    {
        //explosion = GetComponent<>();
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(1.3f);
        this.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("EXPLODED!!!!!");
        Debug.Log(other.gameObject.tag);
        //Hack to prevent it from exploding TWICE
        if (!enabled)
        {
            return;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            enabled = false;
            Debug.Log("Stepped on mine!");
            //Explode
            //TODO less damage if you are FAR
            other.gameObject.GetComponent<HealthController>().TakeDamage(30f, this.gameObject);
            //GameObject particle = Instantiate(explosiveParticle, transform.position, Quaternion.identity); 
            explosion.Emit(20);
            EventManager.TriggerEvent<MineExplodeEvent, Vector3>(transform.position);
            StartCoroutine(SelfDestruct());
//            ParticleEffect pe = particle.GetComponent<ParticleEffect>();
//            pe.looping = false;
        }
    }
}