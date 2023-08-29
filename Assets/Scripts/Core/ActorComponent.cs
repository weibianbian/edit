namespace RailShootGame
{
    public abstract class ActorComponent : ReferencePoolObject
    {
        protected Actor Outer;

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
        public void RegisterComponentWithWorld(UWorld World)
        {
            ExecuteRegisterEvents();
        }
        public void ExecuteRegisterEvents()
        {
            OnRegister();
        }
        public virtual void OnRegister()
        {

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

