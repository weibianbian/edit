using CopyBT;
using System.Collections.Generic;

namespace BT.Editor
{
    public class EventNode : BehaviourNode
    {
        public float priority = 0;
        bool triggered = false;
        public void OnEvent()
        {
            if (status == ENodeStatus.RUNNING)
            {
                ChildAtIndex(0).Reset();
            }
            triggered = true;
            //self.inst.brain:ForceUpdate()

        }
    }
}
