using System;

namespace BT.Runtime
{
    public class BTTaskNode : BTNode
    {
        public Action action;
        protected override void OnVisit()
        {
            action?.Invoke();
            status = ENodeStatus.SUCCESS;
        }
    }
    [Serializable]
    public class BTMoveTo : BTTaskNode
    {
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
