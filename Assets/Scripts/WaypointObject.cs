using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WaypointObject : MonoBehaviour
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
public class WaypointGroup : MonoBehaviour
{
    public List<WaypointObject> waypoints=new List<WaypointObject>();
}