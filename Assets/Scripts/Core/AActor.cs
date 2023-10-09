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
        //������ʱ����Ҫ���������Ϣ
        public void Spawn()
        {
            move.StopMove(EMoveStatus.MOVE_STATUS_DONE);
        }

        public void PostSpawnInitialize()
        {
            //�����һ����������
            // - Actor���û�����
            // - Actor��ȡPreInitializeComponents()
            // - Actor��������Ȼ���������ȫ��װ����
            // - Actor�����ȡOnComponentCreated
            // - Actor�����ȡInitializeComponent
            // -��һ�ж����úú�Actor����PostInitializeComponents()����
            //

            //�ӳ����ɺͷ��ӳ����ɵ�����Ӧ������ͬ��
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
        //֪ͨ�ű������ѱ����������ֵ�Ͳ����
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

