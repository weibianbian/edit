using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

public class GameConst
{
    public const int ReturnToParent = -2;
    public const int NotInitialized = -1;
}
public class WaypointAgent:SerializedMonoBehaviour
{
    private Color color = new Color(1, 0, 0);
    public float radius = 0.25f;

    [ShowInInspector]
    [HideReferenceObjectPicker]
    public CustomEventController eventController=new CustomEventController();
    public void Start()
    {
        eventController.StartEvent();
    }
    public void Update()
    {
        eventController.Update(Time.deltaTime);
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);
        //if (waypoint != null)
        //{
        //    waypoint.position = transform.position;
        //}
        
    }
}