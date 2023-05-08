public class FSMActionStateGraph: FSMStateGraph
{
    public override StateBase CreateFSMFromGraph()
    {
        return new ActionState();
    }
}
