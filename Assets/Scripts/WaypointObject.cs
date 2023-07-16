using System.Collections.Generic;
using UnityEngine;

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
    public UnityEngine.Color color = new Color(0, 1, 1, 0.9f);
    public float arrowHeadLength = 1.0f;
    public bool colorizeWaypoints = true;
    public List<WaypointObject> waypoints=new List<WaypointObject>();

    private WaypointObject[] FindChildrenPoints()
    {
        var childrenWaypoints = GetComponentsInChildren<WaypointObject>();

        if (childrenWaypoints.Length > 0)
        {
            var ChildrenPoints = new WaypointObject[childrenWaypoints.Length];

            var Iter = childrenWaypoints.GetEnumerator();

            int i = 0;

            while (Iter.MoveNext())
            {
                ChildrenPoints[i] = Iter.Current as WaypointObject;
                //ChildrenPoints[i].AccessIndex = i;
                ++i;
            }

            return ChildrenPoints;
        }

        return null;
    }
    void OnDrawGizmos()
    {
        var drawPoints = FindChildrenPoints();

        if (drawPoints != null && drawPoints.Length > 0)
        {
            Gizmos.color = color;

            for (int i = 0; i < drawPoints.Length - 1; ++i)
            {
                var Start = drawPoints[i].gameObject.transform.position;
                var End = drawPoints[i + 1].gameObject.transform.position;

                var direction = (End - Start).normalized;

                var Length = Vector3.Distance(End, Start) - drawPoints[i + 1].radius - drawPoints[i].radius;

                Start = Start + direction * drawPoints[i].radius;
                End = Start + direction * Length;

                DrawArrowHelper.Draw(
                    Start,
                    End,
                    arrowHeadLength
                    );

                //if (colorizeWaypoints)
                //{
                //    drawPoints[i + 1].color = color;
                //}
            }

            // draw start icon
            Gizmos.DrawIcon(
                    new Vector3(drawPoints[0].transform.position.x,
                        drawPoints[0].transform.position.y + drawPoints[0].radius * 3.5f,
                        drawPoints[0].transform.position.z
                        ),
                    "StartPoint",
                    true
                );

            // draw end icon
            Gizmos.DrawIcon(
                    new Vector3(drawPoints[drawPoints.Length - 1].transform.position.x,
                        drawPoints[drawPoints.Length - 1].transform.position.y + drawPoints[drawPoints.Length - 1].radius * 3.5f,
                        drawPoints[drawPoints.Length - 1].transform.position.z
                        ),
                    "EndPoint",
                    true
                );
        }
    }
}