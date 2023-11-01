using System.Collections.Generic;
using UnityEngine;

namespace RailShootGame
{
    public class PathData
    {
        public List<PathPoint> PathPoints = new List<PathPoint>();
        public bool IsValid()
        {
            return true;
        }
        public List<PathPoint> GetPathPoints()
        {
            return PathPoints;
        }
        public bool IsValidIndex(int Index)
        {
            return Index >= 0 && PathPoints.Count > Index;
        }
        public Vector3 GetPathPointLocation(int Index)
        {
            PathPoint point= PathPoints[Index];
            return point.Location;
        }
    }
}

