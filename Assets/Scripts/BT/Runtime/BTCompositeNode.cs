using GraphProcessor;
using UnityEngine;

namespace BehaviorTree.Runtime
{
    public class BTCompositeNode: BTNode
    {
        [Input(name = "", allowMultiple=false), Vertical]
        public ENodeResult input;
        [Output("", true), Vertical]
        public ENodeResult output;
        public override bool isRenamable => false;

        public virtual int GetChild(int preChild,ENodeResult lastResult)
        {
            return 0;
        }
        public int GetChildrenNum()
        {
            if (outputPorts.Count > 0)
            {
                return outputPorts[0].GetEdges().Count;
            }
            return 0;
        }
    }
}

