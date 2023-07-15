namespace RailShootGame
{
    public abstract class ActorCompt
    {
        public Actor owner;

        public ActorCompt(Actor owner)
        {
            this.owner = owner;
        }
        public virtual void Update()
        {

        }
    }
}

