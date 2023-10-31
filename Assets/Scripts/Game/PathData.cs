using System.Collections.Generic;

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
    }
}

