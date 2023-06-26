using BT.Runtime;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public BehaviorTreeGraph so;
    public void Start()
    {
        if (so == null)
        {
            LoadTree(so.behaviorTree);
        }
    }
    public void MoveToPosition(Vector3 pos)
    {
        GetComponent<MovementCompt>().MoveToPosition(pos);
    }
    public bool ReachedPos(Vector3 pos)
    {
       return GetComponent<MovementCompt>().ReachedPos(pos);
    }
    public void LoadTree(BehaviorTree assets)
    {

    }
}
