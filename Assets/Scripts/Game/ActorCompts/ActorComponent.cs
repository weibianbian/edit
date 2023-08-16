namespace RailShootGame
{
    public abstract class ActorComponent
    {
        public Actor owner;

        public ActorComponent(Actor owner)
        {
            this.owner = owner;
        }
        public virtual void TickComponent()
        {

        }
    }
}

