using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject projectile;
    public float projectSpeed;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.B))
        {
            GameObject fireBall = Instantiate(projectile, transform) as GameObject;
            Rigidbody rb = fireBall.GetComponent<Rigidbody>();
            rb.velocity = this.transform.forward * projectSpeed;
        }
    }
}