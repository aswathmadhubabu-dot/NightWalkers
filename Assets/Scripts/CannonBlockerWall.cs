using UnityEngine;

public class CannonBlockerWall : MonoBehaviour
{
    public float health;

    private void Update()
    {
        // Debug.Log(health);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}