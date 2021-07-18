using UnityEngine;

public class GoalBehaviourScript : MonoBehaviour
{
    public GameObject ball;
    public MatchController match;

    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ball && match != null && !match.recentGoal)
        {
            //Goal!
            match.NewGoal();
            EventManager.TriggerEvent<HitGoalEvent, Vector3>(other.transform.position);
        }
    }
}