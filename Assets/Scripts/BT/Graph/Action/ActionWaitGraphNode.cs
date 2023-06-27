using BT.Runtime;
using GraphProcessor;

namespace BT.Graph
{
    [System.Serializable, NodeMenuItem("BT/Action/Wait")]
    public class ActionWaitGraphNode : ActionGraphNode
    {
        public BTWaitNodeData data=new BTWaitNodeData();
        public ActionWaitGraphNode()
        {
            classData = typeof(BTWaitAction);
        }
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
