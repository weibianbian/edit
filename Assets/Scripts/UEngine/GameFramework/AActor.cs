using System.Collections.Generic;
using UEngine.Components;
using UEngine.Core;
using UnityEngine;

namespace UEngine.GameFramework
{
    //����·��
    //1��SpawnActor ������ 
    //2��PostSpawnInitialize
    //3��PostActorCreated �����󼴱����ɵ�Actor���ã�������������Ϊ�ڴ˷�����PostActorCreated��PostLoad����
    //4��ExecuteConstruction
    ////////////OnConstruction
    //5��PostActorConstruction
    ////////////1��PreInitializeComponents--��Actor������ϵ���InitializeComponent֮ǰ���е���
    ////////////2��InitializeComponent--Actor�϶����ÿ������Ĵ�����������
    ////////////3��PostInitializeComponents--Actor�������ʼ�������
    //6��OnActorSpawned
    //7��BeginPlay
    //Spawn -----  Init  ----Activate
    public class AActor : ReferencePoolObject
    {
        public ActorObject actorObject;
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
        public T CreateDefaultSubobject<T>() where T:ActorComponent
        {
            ActorComponent result = ReferencePool.Acquire(typeof(T)) as ActorComponent;
            result.SetOwner(this);
            result.PostInitProperties();

            return result as T;
        }
        public void Init(UWorld game)
        {
          
        }
        //������ʱ����Ҫ���������Ϣ
        public void Spawn()
        {
            //move.StopMove(EMoveStatus.MOVE_STATUS_DONE);
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
        public virtual void Tick(float DeltaTime)
        {
            foreach (var item in OwnedComponents)
            {
                item.TickComponent(DeltaTime);
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

