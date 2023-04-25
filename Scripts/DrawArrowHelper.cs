using UnityEngine;

public class DrawArrowHelper
{
    public static void Draw(Vector3 StartPos, Vector3 EndPos, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Gizmos.DrawLine(StartPos, EndPos);
        var direction = (EndPos - StartPos).normalized;

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);

        Gizmos.DrawRay(EndPos, right * arrowHeadLength);
        Gizmos.DrawRay(EndPos, left * arrowHeadLength);
    }
}
