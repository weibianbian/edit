public class MeActionState<TEvent> : MeStateBase, IMeActionable<TEvent>
{
    public void OnAction(TEvent trigger)
    {
       
    }

    public void OnAction<TData>(TEvent trigger, TData data)
    {
    }
    public override void OnExitRequest()
    {
        if (!needsExitTime)
        {
            fsm.StateCanExit();
        }
    }
}
