using BT.Runtime;
using GraphProcessor;

namespace BT.Editor
{
    [System.Serializable, NodeMenuItem("BT/Action/Wait")]
    public class ActionWait : ActionNode
    {
        public WaitNodeData data=new WaitNodeData();
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
