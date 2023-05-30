﻿using GraphProcessor;
using System;

namespace BehaviorTree.Runtime
{
    public class BTCompositeNodeSelector : BTCompositeNode
    {
        public override int GetChild(int prevChild, ENodeResult lastResult)
        {
            int nextChildIdx = BTSpecialChild.ReturnToParent;

            if (prevChild == BTSpecialChild.NotInitialized)
            {
                nextChildIdx = 0;
            }
            else if (lastResult == ENodeResult.Failed && (prevChild + 1) < GetChildrenNum())
            {
                nextChildIdx = prevChild + 1;
            }

            return nextChildIdx;
        }
    }
   public static class BTSpecialChild
    {
        public static int ReturnToParent = -2;
        public static int NotInitialized = -1;
    }
}

