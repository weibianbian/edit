using System.Collections.Generic;

namespace CopyBT
{
    public class EventNode : BehaviourNode
    {
        public float priority = 0;
        public EventNode(string eventName, BehaviourNode child, float priority) : base(eventName, new List<BehaviourNode>() { child })
        {
            this.priority = priority;

        }
    }
}

