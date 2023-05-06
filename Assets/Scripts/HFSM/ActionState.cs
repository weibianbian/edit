public class ActionState : StateBase, IActionable
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
public class MoveActionState: ActionState
{

}
public class TurnToActionState : ActionState
{

}
