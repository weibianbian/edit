using System.Collections.Generic;
using UEngine.Components;
using UEngine.Core;
using UnityEngine;

namespace UEngine.GameFramework
{
    //生成路径
    //1、SpawnActor 被调用 
    //2、PostSpawnInitialize
    //3、PostActorCreated 创建后即被生成的Actor调用，构建函数类行为在此发生，PostActorCreated和PostLoad互斥
    //4、ExecuteConstruction
    ////////////OnConstruction
    //5、PostActorConstruction
    ////////////1、PreInitializeComponents--在Actor的组件上调用InitializeComponent之前进行调用
    ////////////2、InitializeComponent--Actor上定义的每个组件的创建辅助函数
    ////////////3、PostInitializeComponents--Actor的组件初始化后调用
    //6、OnActorSpawned
    //7、BeginPlay
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
        //创建的时候，需要填充配置信息
        public void Spawn()
        {
            //move.StopMove(EMoveStatus.MOVE_STATUS_DONE);
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

