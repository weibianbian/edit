using UnityEngine;

public class MyBounds
{
    public Vector3 min;
    public Vector3 max;
    public MyBounds(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }

    public MyBounds(Vector3 point)
    {
        this.min = point;
        this.max = point;
    }

    public bool ContainsPoint(Vector3 point)
    {
        if ( point.x < min.x || point.y < min.y || point.z < min.z
             || point.x > max.x || point.y > max.y || point.z > max.z ) {
            return false;
        }
        return true;
    }

    public void TranslateSelf(Vector3 translation)
    {
        min += translation;
        max += translation;
    }
}