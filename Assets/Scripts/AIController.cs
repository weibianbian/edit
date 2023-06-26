using UnityEngine;

public class AIController : MonoBehaviour
{
    public ScriptableObject so;
    public void Start()
    {
        if (so == null)
        {

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
}
