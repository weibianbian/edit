using UnityEngine;
using UnityEngine.AI;

namespace RailShootGame
{
    public class MovementCompt
    {
        public Actor owner;
        public NavMeshAgent agent;
        public Vector3 targetPos;

        public MoveState move;

        public MovementCompt(Actor owner)
        {
            this.owner = owner;
        }

        public void Init()
        {
            move = new MoveState();
        }
        

        public void Update()
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
            if (GetMovePos(owner.GetOrigin()))
            {


            }
            if (move.moveCommand == EMoveCommand.MOVE_TO_POSITION)
            {
                //if (move.moveStatus == EMoveStatus.MOVE_STATUS_MOVING)
                //{
                //    if (ReachedPos(move.moveDest))
                //    {
                //        StopMove(EMoveStatus.MOVE_STATUS_DONE);
                //    }
                //}
            }
        }
        public bool GetMovePos(Vector3 seekPos)
        {
            switch (move.moveCommand)
            {
                case EMoveCommand.MOVE_NONE:
                    break;
                case EMoveCommand.MOVE_FACE_ENEMY:
                    break;
                case EMoveCommand.MOVE_FACE_ENTITY:
                    break;
                case EMoveCommand.NUM_NONMOVING_COMMANDS:
                    break;
                case EMoveCommand.MOVE_TO_ENEMYHEIGHT:
                    break;
                case EMoveCommand.MOVE_TO_ENTITY:
                    break;
                case EMoveCommand.MOVE_OUT_OF_RANGE:
                    break;
                case EMoveCommand.MOVE_TO_ATTACK_POSITION:
                    break;
                case EMoveCommand.MOVE_TO_COVER:
                    break;
                case EMoveCommand.MOVE_TO_POSITION:
                    break;
                case EMoveCommand.MOVE_TO_POSITION_DIRECT:
                    break;
                case EMoveCommand.MOVE_SLIDE_TO_POSITION:
                    break;
                case EMoveCommand.MOVE_WANDER:
                    break;
                case EMoveCommand.NUM_MOVE_COMMANDS:
                    break;
                default:
                    break;
            }
            if (ReachedPos(move.moveDest, move.moveCommand))
            {
                StopMove(EMoveStatus.MOVE_STATUS_DONE);
                return false;
            }
            return true;
        }
        public void MoveToPosition(Vector3 pos)
        {
            if (ReachedPos(pos, move.moveCommand))
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
            move.moveCommand = EMoveCommand.MOVE_NONE;
            move.moveStatus = status;
            move.moveDest = owner.GetOrigin();
            move.moveDir = Vector3.zero;
        }

        public bool ReachedPos(Vector3 pos, EMoveCommand moveCommand)
        {
            MyBounds bnds = new MyBounds(new Vector3(-16f / 1000, float.MinValue, -8f / 1000), new Vector3(16 / 1000, float.MaxValue, 64 / 1000));
            bnds.TranslateSelf(owner.GetOrigin());
            if (bnds.ContainsPoint(pos))
            {
                return true;
            }

            return false;
        }
    }
}

