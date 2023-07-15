using HFSMRuntime;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace RailShootGame
{
    public class Actor
    {
        public ActorObject actorObject;
        public MovementCompt move;
        public Vector3 position;
        public Game game;


        public HierarchicalStateMachine hsm;
        public void Init(Game game)
        {
            move = new MovementCompt(this);
            move.Init();

            hsm = new HierarchicalStateMachine(game);
        }
        public void Spawn()
        {
            move.StopMove(EMoveStatus.MOVE_STATUS_DONE);
        }
        public Vector3 GetOrigin()
        {
            return actorObject.gameObject.transform.position;
        }
        public void Update()
        {
            move?.Update();
        }
        public void InitHFSM()
        {
        }
    }
}

