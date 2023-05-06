public class MeActionState : MeStateBase, IMeActionable
{
    public void OnAction(string trigger)
    {
       
    }

    public void OnAction<TData>(string trigger, TData data)
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
public class MoveActionState: MeActionState
{

}
public class TurnToActionState : MeActionState
{

}
