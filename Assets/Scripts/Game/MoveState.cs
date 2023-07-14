using UnityEngine;

namespace RailShootGame
{
    public class MoveState
    {
        public Vector3 moveDest;
        public Vector3 moveDir;
        public EMoveType moveType;
        public EMoveCommand moveCommand;
        public EMoveStatus moveStatus;
    }
}

