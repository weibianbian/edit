using GraphProcessor;
using CopyBT;
using UnityEngine;

namespace BT.GraphProcessor
{
    [System.Serializable, NodeMenuItem("BT/Action/MoveTo")]
    public class MoveTo : ActionNode
    {
        public Vector3 target;
        protected override void OnVisit()
        {
            if (status == ENodeStatus.READY)
            {
                target= new Vector3(Random.Range(0f, 100f), 0, Random.Range(0f, 100f));
                if (ownerTreeManager.aiController.ReachedPos(target))
                {
                    status = ENodeStatus.SUCCESS;
                    
                }
                else
                {
                    ownerTreeManager.aiController.MoveToPosition(target);
                    status = CopyBT.ENodeStatus.RUNNING;
                }
            }
            else if (status == ENodeStatus.RUNNING)
            {
                if (ownerTreeManager.aiController.ReachedPos(target))
                {
                    status = ENodeStatus.SUCCESS;
                }
            }
           
        }
    }
}
