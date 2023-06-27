using BT.Runtime;

namespace BT.Graph
{
    [System.Serializable, TreeNodeMenuItem("BT/Composite/ParallelNode")]
    public class ParallelGraphNode : CompositieGraphNode
    {
        public ParallelGraphNode()
        {
            classData = typeof(BTParallelCompositieNode);
        }
        public override string name => "并行结点";
        public override void Step()
        {
            if (status != ENodeStatus.RUNNING)
            {
                Reset();
            }
            else
            {
                for (int i = 0; i < ChildCount; i++)
                {
                    if (ChildAtIndex(i).status == ENodeStatus.SUCCESS && ChildAtIndex(i) is ConditionGraphNode)
                    {
                        ChildAtIndex(i).Reset();
                    }
                }
            }
        }
        protected override void OnVisit()
        {
            bool done = true;
            for (int i = 0; i < ChildCount; i++)
            {
                BehaviorGraphNode child = ChildAtIndex(i);
                if (child.status != ENodeStatus.SUCCESS)
                {
                    child.Visit();
                    if (child.status == ENodeStatus.FAILED)
                    {
                        status = ENodeStatus.FAILED;
                        return;
                    }
                }
                if (child.status == ENodeStatus.RUNNING)
                {
                    done = false;
                }
            }
            if (done)
            {
                status = ENodeStatus.SUCCESS;
            }
            else
            {
                status = ENodeStatus.RUNNING;
            }
        }
    }
}
