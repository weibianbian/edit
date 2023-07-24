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
    //Spawn -----  Init  ----Activate
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
        //创建的时候，需要填充配置信息
        public void Spawn()
        {
            move.StopMove(EMoveStatus.MOVE_STATUS_DONE);
        }
        //通知脚本怪物已被触发器或手电筒激活
        public void Activate()
        {

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
            move?.TickComponent();
            hsm?.TickComponent();
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

