using System;

namespace BT.Runtime
{
    public class BTActionNode : BTNode
    {
        public Action action;
        protected override void OnVisit()
        {
            action?.Invoke();
            status = ENodeStatus.SUCCESS;
        }
    }
    [Serializable]
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
