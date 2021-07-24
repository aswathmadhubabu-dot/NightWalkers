using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject explosion;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        // Destroy(gameObject);
        if (other.CompareTag("ExitWall"))
        {
            Destroy(gameObject);
            Instantiate(explosion, transform.position, transform.rotation);
        }
    }
}