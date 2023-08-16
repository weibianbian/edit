using HFSMRuntime;

namespace RailShootGame
{
    public class HierarchicalStateMachineCompt : ActorComponent
    {
        public HierarchicalStateMachine hsm;
        public HierarchicalStateMachineCompt(Actor owner) : base(owner)
        {
            this.owner = owner;
        }
        public void Init()
        {
            State l = new State(EStatus.Patrol.ToString(), null);

            State m = new State(EStatus.Combat.ToString(), null);

            l.AddTransition(new Transition(new SoundSensorCondition(), m, 0));
            hsm = new HierarchicalStateMachine(owner.game, l, m);
        }

        public override void TickComponent()
        {
            hsm.Update(owner.game, owner);
        }

    }
}

