using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class GameConst
{
    public const int ReturnToParent = -2;
}

public class WaypointAgent:SerializedMonoBehaviour
{
    private Color color = new Color(1, 0, 0);
    public float radius = 0.25f;
    [ShowInInspector]
    [HideReferenceObjectPicker]
    [ListDrawerSettings(CustomAddFunction = "AddCustomEventAction")]
    public List<CustomEvent> customEvents = new List<CustomEvent>();
    public CustomEvent AddCustomEventAction => new CustomEvent();

    public CustomEvent nextEvent = null;
    public int currentIndex = -1;
    public bool bRequestedFlowUpdate = false;
    public const int ReturnToParent = -2;
    
    public void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);
        //if (waypoint != null)
        //{
        //    waypoint.position = transform.position;
        //}
        
    }
    public void StartExecuteList()
    {
        currentIndex = -1;

        int index = GetNext(currentIndex);
    }
    public int GetNext(int lastIndex)
    {
        int nextChildIndex = GameConst.ReturnToParent;
        if (lastIndex + 1 < customEvents.Count)
        {
            nextChildIndex = (lastIndex + 1);
        }
        return nextChildIndex;
    }
    public void Update()
    {
        if (bRequestedFlowUpdate)
        {
            bRequestedFlowUpdate = false;
        }
    }
 
}