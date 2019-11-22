using UnityEngine;

public class GoalBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameEvent PlayerReachGoal;

    private void OnCollisionEnter(Collision collision)
    {
        foreach (var item in collision.contacts)
        {
            if (item.otherCollider.CompareTag(Global.PLAYERTAG))
                PlayerReachGoal.Raise();
        }
    }
}
