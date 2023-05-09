using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

public class GameConst
{
    public const int ReturnToParent = -2;
    public const int NotInitialized = -1;
}
public class WaypointGraph:SerializedMonoBehaviour
{
    private Color color = new Color(1, 0, 0);
    public float radius = 0.25f;

   
    public void Start()
    {
    }
    public void Update()
    {
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);
    }
}