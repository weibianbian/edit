using CopyBT;
using GraphProcessor;

namespace BT.GraphProcessor
{
    [System.Serializable, NodeMenuItem("BT/Composite/ParallelNode")]
    public class ParallelNode : CompositieNode
    {
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
                    if (ChildAtIndex(i).status == ENodeStatus.SUCCESS && ChildAtIndex(i) is Condition)
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
                BehaviourNode child = ChildAtIndex(i);
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
