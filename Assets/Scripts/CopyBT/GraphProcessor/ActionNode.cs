using GraphProcessor;
using System;

namespace CopyBT.GraphProcessor
{
    [System.Serializable, NodeMenuItem("BT/ActionNode")]
    public class ActionNode : BehaviourNode
    {
        [Input(name = "", allowMultiple = false), Vertical]
        public ENodeStatus input;
        public Action action;
        public override void Visit()
        {
            action.Invoke();
            status = ENodeStatus.SUCCESS;
        }
    }
}
