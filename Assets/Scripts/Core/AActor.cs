using GameplayAbilitySystem;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace RailShootGame
{
    public interface IActorLogic
    {

        void UpdateLogic(float delta);
    }
    public interface IActorComponent
    {
        void UpdateLogic(float delta);
    }
    public class Pawn : AActor
    {

    }

    //Spawn -----  Init  ----Activate
    public class AActor : ReferencePoolObject
    {
        public ActorObject actorObject;
        public MovementCompt move;
        public SensorCompt sensor;
        public HierarchicalStateMachineCompt hsm;
        public Vector3 position;
        public ULevel Outer;

        private HashSet<ActorComponent> OwnedComponents=new HashSet<ActorComponent>();

        public HashSet<ActorComponent> GetInstanceComponents()
        {
            return OwnedComponents;
        }
        public void AddOwnedComponent(ActorComponent Component)
        {
            if (OwnedComponents.Add(Component))
            {

            };
        }
        public void Init(UWorld game)
        {
            move = new MovementCompt();
            move.Init();

            sensor = new SensorCompt();
            sensor.AddSensor(new SoundSensor());

            hsm = new HierarchicalStateMachineCompt();
            hsm.Init();
        }
        //创建的时候，需要填充配置信息
        public void Spawn()
        {
            move.StopMove(EMoveStatus.MOVE_STATUS_DONE);
        }

        public void PostSpawnInitialize()
        {
            //这里的一般流程如下
            // - Actor设置基础。
            // - Actor获取PreInitializeComponents()
            // - Actor构建自身，然后将其组件完全组装起来
            // - Actor组件获取OnComponentCreated
            // - Actor组件获取InitializeComponent
            // -当一切都设置好后，Actor会获得PostInitializeComponents()方法
            //

            //延迟生成和非延迟生成的序列应该是相同的
            UWorld World =GetWorld();

            RegisterAllComponents();
            FinishSpawning();
        }
        public void FinishSpawning()
        {
            ExecuteConstruction();
            PostActorConstruction();
        }
        public void ExecuteConstruction()
        {

        }
        public void PostActorConstruction()
        {
            PostInitializeComponents();

            InitializeComponents();
        }
        public virtual void PostInitializeComponents()
        {
        }
        public void InitializeComponents()
        {
            foreach (var ActorComp in OwnedComponents)
            {
                ActorComp.InitializeComponent();
            }
        }
        public void RegisterAllComponents()
        {
            IncrementalRegisterComponents();
        }
        public bool IncrementalRegisterComponents()
        {
            UWorld World = GetWorld();
            foreach (var Component in OwnedComponents)
            {
                Component.RegisterComponentWithWorld(World);
            }
            return true;
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
            move?.TickComponent(Time.deltaTime);
            hsm?.TickComponent(Time.deltaTime);
            if (Input.GetKeyDown(KeyCode.J))
            {
                (sensor.GetSensor(Sensor.ESensorType.Sound) as SoundSensor).gunSound = true;
            }
        }
        public void InitHFSM()
        {
        }
        public UWorld GetWorld()
        {
            return Outer.OwningWorld;
        }
    }
}

