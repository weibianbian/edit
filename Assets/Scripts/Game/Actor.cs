using UnityEngine;
using UnityEngine.AI;

namespace RailShootGame
{
    public class Actor
    {
        public MovementCompt move;
    }
    public class MovementCompt
    {
        public NavMeshAgent agent;
        public Vector3 targetPos;

        public MoveState move;

        void Init()
        {
            move = new MoveState();
            //MoveToPosition(targetPos.transform.position);
        }

        void Update()
        {
            switch (move.moveType)
            {
                case EMoveType.MOVETYPE_DEAD:
                    break;
                case EMoveType.MOVETYPE_ANIM:
                    AnimMove();
                    break;
                case EMoveType.MOVETYPE_SLIDE:
                    break;
                case EMoveType.MOVETYPE_FLY:
                    break;
                case EMoveType.MOVETYPE_STATIC:
                    break;
                case EMoveType.NUM_MOVETYPES:
                    break;
                default:
                    break;
            }
        }

        public void AnimMove()
        {
            if (move.moveCommand == EMoveCommand.MOVE_TO_POSITION)
            {
                if (move.moveStatus == EMoveStatus.MOVE_STATUS_MOVING)
                {
                    if (ReachedPos(move.moveDest))
                    {
                        StopMove(EMoveStatus.MOVE_STATUS_DONE);
                    }
                }
            }
        }

        public void MoveToPosition(Vector3 pos)
        {
            if (ReachedPos(pos))
            {
                StopMove(EMoveStatus.MOVE_STATUS_DONE);
                return;
            }

            move.moveDest = pos;
            move.moveType = EMoveType.MOVETYPE_ANIM;
            move.moveStatus = EMoveStatus.MOVE_STATUS_MOVING;
            move.moveCommand = EMoveCommand.MOVE_TO_POSITION;
            agent.enabled = true;
            agent.SetDestination(pos);
        }

        public void StopMove(EMoveStatus status)
        {
            agent.enabled = false;
        }

        public bool ReachedPos(Vector3 pos)
        {
            MyBounds bnds = new MyBounds(new Vector3(-16f / 1000, float.MinValue, -8f / 1000), new Vector3(16 / 1000, float.MaxValue, 64 / 1000));
            //bnds.TranslateSelf(gameObject.transform.position);
            if (bnds.ContainsPoint(pos))
            {
                return true;
            }

            return false;
        }
    }
}

