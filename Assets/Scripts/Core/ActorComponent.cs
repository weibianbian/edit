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

