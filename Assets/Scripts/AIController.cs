using BT.Runtime;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public void Start()
    {
       
    }
    public void MoveToPosition(Vector3 pos)
    {
        //GetComponent<MovementCompt>().MoveToPosition(pos);
    }
    public bool ReachedPos(Vector3 pos)
    {
        return true;
       //return GetComponent<MovementCompt>().ReachedPos(pos);
    }
    public void LoadTree(BehaviorTree assets)
    {

    }
}
