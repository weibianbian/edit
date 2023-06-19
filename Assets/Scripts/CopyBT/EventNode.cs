using System.Collections.Generic;

namespace CopyBT
{
    public class EventNode : CopyBTBehaviourNode
    {
        public float priority = 0;
        bool triggered = false;
        public EventNode(string eventName, CopyBTBehaviourNode child, float priority) : base(eventName, new List<CopyBTBehaviourNode>() { child })
        {
            this.priority = priority;

        }
        public void OnEvent()
        {
            if (status == ENodeStatus.RUNNING)
            {
                children[0].Reset();
            }
            triggered = true;
            //self.inst.brain:ForceUpdate()

        }
    }
}

