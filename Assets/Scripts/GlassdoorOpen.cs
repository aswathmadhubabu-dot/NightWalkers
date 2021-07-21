using UnityEngine;

public class GlassdoorOpen : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator doorOpenAnimator;

    void Start() => doorOpenAnimator = this.GetComponent<Animator>();

    private void OnCollisionEnter(Collision other) => doorOpenAnimator.SetBool("character_nearby", true);

    private void OnCollisionExit(Collision other) => doorOpenAnimator.SetBool("character_nearby", false);
}