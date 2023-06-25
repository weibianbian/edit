using BT.Runtime;
using GraphProcessor;

namespace BT.Editor
{
    [System.Serializable, NodeMenuItem("BT/Action/Wait")]
    public class ActionWaitGraph : ActionGraphNode
    {
        public BTWaitNodeData data=new BTWaitNodeData();
        public override string name => "等待指定时间";
        protected override void OnVisit()
        {
            if (status == ENodeStatus.READY)
            {
                
            }
            else if (status == ENodeStatus.RUNNING)
            {
                //if (ownerTreeManager.aiController.ReachedPos(data.target))
                //{
                //    status = ENodeStatus.SUCCESS;
                //}
            }
        }
    }
}
