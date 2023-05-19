using System.Collections.Generic;

namespace CopyBT
{
    public class PriorityNode : BehaviourNode
    {
        public float period = 0;
        public PriorityNode(List<BehaviourNode> children, float period = 1) : base("Priority", children)
        {
            this.period = period;
        }
    }
    public class Follow : BehaviourNode
    {
        public Follow(string name) : base("Follow")
        {
        }
    }
}

