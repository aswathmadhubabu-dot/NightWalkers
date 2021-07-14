using UnityEngine;

public class SaveGoal : MonoBehaviour
{
    public GameObject goalKeeper;
    [SerializeField] private Animator animator;

    void Start()
    {
        animator = goalKeeper.GetComponent<Animator>();
        Debug.Log(animator);
    }

    private void OnTriggerEnter(Collider other) => animator.SetBool("isBallOnTop", true);

    private void OnTriggerExit(Collider other) => animator.SetBool("isBallOnTop", false);
}