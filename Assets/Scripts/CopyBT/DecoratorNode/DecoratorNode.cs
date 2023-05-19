using System.Collections.Generic;

namespace CopyBT
{
    public class DecoratorNode : BehaviourNode
    {
        public DecoratorNode(string name, BehaviourNode child) : base(name, new List<BehaviourNode>() { child })
        {

        }
    }
}

