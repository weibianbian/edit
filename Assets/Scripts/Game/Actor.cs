using HFSMRuntime;
using UnityEngine;

namespace RailShootGame
{
    public class Sensor
    {
        public enum ESensorType
        {
            Sound,
        }
        public ESensorType sensorType;
    }
    public class SoundSensor : Sensor
    {
        public bool gunSound;
        public SoundSensor()
        {
            sensorType=Sensor.ESensorType.Sound;
        }

    }
    public class Actor
    {
        public ActorObject actorObject;
        public MovementCompt move;
        public SensorCompt sensor;
        public Vector3 position;
        public Game game;


        public HierarchicalStateMachine hsm;
        public void Init(Game game)
        {
            move = new MovementCompt(this);
            move.Init();

            sensor = new SensorCompt(this);
            sensor.AddSensor(new SoundSensor());

            State l = new State(EStatus.Patrol.ToString(), null);

            State m = new State(EStatus.Combat.ToString(), null);

            l.AddTransition(new Transition(new SoundSensorCondition(), m, 0));
            hsm = new HierarchicalStateMachine(game, l, m);
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
            hsm.Update(game,this);
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

