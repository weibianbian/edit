using CopyBT;
using GraphProcessor;
using UnityEditor.Experimental.GraphView;

namespace BehaviorTree.Runtime
{
    public class BTCompositeNodeSequence : BTCompositeNode
    {
        public BTCompositeNodeSequence()
        {
        }
        public override void OnNodeCreated()
        {
            base.OnNodeCreated();
        }
        public override int GetChild(int prevChild, ENodeStatus lastResult)
        {
            int nextChildIdx = BTSpecialChild.ReturnToParent;

            if (prevChild == BTSpecialChild.NotInitialized)
            {
                nextChildIdx = 0;
            }
            else if (lastResult == ENodeStatus.SUCCESS && (prevChild + 1) < GetChildrenNum())
            {
                nextChildIdx = prevChild + 1;
            }

            return nextChildIdx;
        }

        
    }
}

