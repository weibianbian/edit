namespace RailShootGame
{
    public abstract class ActorComponent : ReferencePoolObject
    {
        protected Actor Outer;
        private UWorld WorldPrivate;
        public ActorComponent()
        {
        }
        public virtual void TickComponent()
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
        public void SetOwner(Actor InOuter)
        {
            Outer = InOuter;
        }
        public Actor GetOwner()
        {
            return Outer;
        }
    }
}

