using HFSMRuntime;

namespace RailShootGame
{
    public class HierarchicalStateMachineCompt : ActorComponent
    {
        public HierarchicalStateMachine hsm;
       
        public void Init()
        {
            State l = new State(EStatus.Patrol.ToString(), null);

            State m = new State(EStatus.Combat.ToString(), null);

            //l.AddTransition(new Transition(new SoundSensorCondition(), m, 0));
            hsm = new HierarchicalStateMachine(Outer.GetWorld(), l, m);
        }

        public override void TickComponent(float DeltaTime)
        {
            hsm.Update(Outer.GetWorld(), Outer);
        }

    }
}

