namespace RailShootGame
{
    public abstract class ActorComponent : ReferencePoolObject
    {
        protected AActor Outer;
        private UWorld WorldPrivate;
        public ActorComponent()
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

