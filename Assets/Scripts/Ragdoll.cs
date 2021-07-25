using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] rigidBodies;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidBodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
        DeactivateRagdoll();
    }

    void DeactivateRagdoll()
    {
        foreach (var rigidbody in rigidBodies)
        {
            rigidbody.isKinematic = true;
        }

        animator.enabled = true;
    }

    public void ActivateRagdoll()
    {
        this.GetComponent<CharacterController>().enabled = false;
        animator.enabled = false;
        foreach (var rigidbody in rigidBodies)
        {
            rigidbody.isKinematic = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}