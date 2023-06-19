using GraphProcessor;
using CopyBT;
using UnityEngine;

namespace BT.GraphProcessor
{
    [System.Serializable, NodeMenuItem("BT/Action/MoveTo")]
    public class MoveTo : ActionNode
    {
        public MoveToActionData data = new MoveToActionData();
        public override string name => "移动到指定地点";
       
        protected override void OnVisit()
        {
            if (status == ENodeStatus.READY)
            {
                data.target = new Vector3(Random.Range(0f, 100f), 0, Random.Range(0f, 100f));
                if (ownerTreeManager.aiController.ReachedPos(data.target))
                {
                    status = ENodeStatus.SUCCESS;

                }
                else
                {
                    ownerTreeManager.aiController.MoveToPosition(data.target);
                    status = CopyBT.ENodeStatus.RUNNING;
                }
            }
            else if (status == ENodeStatus.RUNNING)
            {
                if (ownerTreeManager.aiController.ReachedPos(data.target))
                {
                    status = ENodeStatus.SUCCESS;
                }
            }

        }
    }
}
