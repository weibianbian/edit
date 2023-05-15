using GraphProcessor;

namespace BehaviorTree.Runtime
{
    [System.Serializable, NodeMenuItem("BT/CompositeNode/Sequence")]
    public class BTCompositeNodeSequence : BTCompositeNode
    {
        public override int GetChild(int prevChild, ENodeResult lastResult)
        {
            int nextChildIdx = BTSpecialChild.ReturnToParent;

            if (prevChild == BTSpecialChild.NotInitialized)
            {
                nextChildIdx = 0;
            }
            else if (lastResult == ENodeResult.Succeeded && (prevChild + 1) < GetChildrenNum())
            {
                nextChildIdx = prevChild + 1;
            }

            return nextChildIdx;
        }
    }
}

