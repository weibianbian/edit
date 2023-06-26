using System;

namespace BT.Runtime
{
    public abstract class BTActionNode : BTNode
    {
        public Action action;
        protected override void OnVisit()
        {
            action?.Invoke();
            status = ENodeStatus.SUCCESS;
        }
    }
    public class BTMoveTo : BTActionNode
    {
        public BTMoveToActionData data = new BTMoveToActionData();
        protected override void OnVisit()
        {
            if (status == ENodeStatus.READY)
            {
              
            }
            else if (status == ENodeStatus.RUNNING)
            {
               
            }
        }
    }
}
