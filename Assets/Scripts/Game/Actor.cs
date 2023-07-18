using HFSMRuntime;
using UnityEngine;

namespace RailShootGame
{
    public interface IActorLogic {

        void UpdateLogic(float delta);
    }
    public interface IActorComponent
    {
        void UpdateLogic(float delta);
    }
    public class Actor
    {
        public ActorObject actorObject;
        public MovementCompt move;
        public SensorCompt sensor;
        public HierarchicalStateMachineCompt hsm;
        public Vector3 position;
        public Game game;



        public void Init(Game game)
        {
            move = new MovementCompt(this);
            move.Init();

            sensor = new SensorCompt(this);
            sensor.AddSensor(new SoundSensor());

            hsm = new HierarchicalStateMachineCompt(this);
            hsm.Init();
        }
        public void Spawn()
        {
            move.StopMove(EMoveStatus.MOVE_STATUS_DONE);
        }
        public Vector3 GetOrigin()
        {
            return actorObject.gameObject.transform.position;
        }
        public GameObject gameObject
        {
            get
            {
                return actorObject ? actorObject.gameObject : null;
            }
        }
        public void Update()
        {
            move?.Update();
            hsm?.Update();
            if (Input.GetKeyDown(KeyCode.J))
            {
                (sensor.GetSensor(Sensor.ESensorType.Sound) as SoundSensor).gunSound = true;
            }
        }
        public void InitHFSM()
        {
        }
    }
}

