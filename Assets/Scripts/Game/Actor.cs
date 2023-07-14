using UnityEngine;

namespace RailShootGame
{
    public class Actor
    {
        public MovementCompt move;
        public Vector3 position;

        public Vector3 GetOrigin()
        {
            return position;
        }
    }
}

