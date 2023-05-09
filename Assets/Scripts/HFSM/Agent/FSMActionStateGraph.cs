public class FSMActionStateGraph: FSMStateGraph
{
    public override StateBase CreateFSMFromGraph(FSMComponentGraph graph)
    {
        return new ActionState(graph.compt);
    }
}
