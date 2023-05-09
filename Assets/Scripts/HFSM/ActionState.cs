public class ActionState : StateBase, IActionable
{
    public ActionState(FSMComponent compt) : base(compt)
    {
    }

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
public class MoveActionState : ActionState
{
    public MoveActionState(FSMComponent compt) : base(compt)
    {
    }
}
public class TurnToActionState : ActionState
{
    public TurnToActionState(FSMComponent compt) : base(compt)
    {
    }
}
