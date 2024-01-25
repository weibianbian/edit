using UEngine.Core;
using UEngine.GameFramework;

namespace UEngine.Components
{
    //UActorComponent::OnComponentCreated
    //UActorComponent::OnRegister
    //UActorComponent::InitializeComponent
    //UActorComponent::BeginPlay
    //UActorComponent::TickComponent
    //UActorComponent::EndPlay
    //UActorComponent::UninitializeComponent
    //UActorComponent::OnUnregister
    //UActorComponent::OnComponentDestroyed
    public abstract class UActorComponent : ReferencePoolObject
    {
        protected AActor Outer;
        private UWorld WorldPrivate;
        public UActorComponent()
        {
        }
        public virtual void TickComponent(float DeltaTime)
        {

        }
        public virtual void InitializeComponent()
        {

        }
        public virtual void PostInitProperties()
        {
            Outer.AddOwnedComponent(this);
        }
        public void RegisterComponentWithWorld(UWorld InWorld)
        {
            ExecuteRegisterEvents();
            WorldPrivate = InWorld;
        }
        public void ExecuteRegisterEvents()
        {
            OnRegister();
        }
        public virtual void OnRegister()
        {

        }
        public virtual UWorld GetWorld()
        {
            return WorldPrivate;
        }
        public void SetOwner(AActor InOuter)
        {
            Outer = InOuter;
        }
        public AActor GetOwner()
        {
            return Outer;
        }
    }
}

