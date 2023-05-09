public class FSMActionStateGraph: FSMStateBaseGraph
{
    protected override StateBase OnCreateFSMFromGraph(FSMComponentGraph graph)
    {
        return new ActionState(graph.compt);
    }
}
