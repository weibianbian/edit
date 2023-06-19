using System.Collections.Generic;

namespace CopyBT
{
    public class DecoratorNode : CopyBTBehaviourNode
    {
        public DecoratorNode(string name, CopyBTBehaviourNode child) : base(name, new List<CopyBTBehaviourNode>() { child })
        {

        }
    }
}

