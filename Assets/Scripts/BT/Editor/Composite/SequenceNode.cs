using BT.Runtime;
using GraphProcessor;

namespace BT.Editor
{
    [System.Serializable, NodeMenuItem("BT/Composite/Sequence")]
    public class SequenceNode : CompositieNode
    {
        public override string name => "序列节点";
        protected override void OnVisit()
        {
            if (status != ENodeStatus.RUNNING)
            {
                idx = 0;
            }
            bool done = false;
            while (idx < ChildCount)
            {
                BehaviourNode child = ChildAtIndex(idx);
                child.Visit();
                if (child.status == ENodeStatus.RUNNING || child.status == ENodeStatus.FAILED)
                {
                    status = child.status;
                    return;
                }
                idx++;
            }
            status = ENodeStatus.SUCCESS;
        }
        public SequenceNodeData NP_SequenceNodeData = new SequenceNodeData { NodeDes = "序列组合器" };
        public override NodeDataBase GetNodeData()
        {
            return NP_SequenceNodeData;
        }
    }
}
