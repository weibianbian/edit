using System;
using System.Collections.Generic;

namespace CopyBT
{
    public static class BehaviourNodeExtension
    {
        public static CopyBTBehaviourNode WhileNode(Func<bool> cond, string name, CopyBTBehaviourNode node)
        {
            return new ParallelNode(new List<CopyBTBehaviourNode>()
            {
                new ConditionNode(cond),
                node
            });
        }
        public static CopyBTBehaviourNode IfNode(Func<bool> cond, string name, CopyBTBehaviourNode node)
        {
            return new SequenceNode(new List<CopyBTBehaviourNode>()
            {
                new ConditionNode(cond),
                node
            });
        }
        public static CopyBTBehaviourNode IfThenDoWhileNode(Func<bool> ifcond, Func<bool> whilecond, string name, CopyBTBehaviourNode node)
        {
            return new ParallelNode(new List<CopyBTBehaviourNode>()
            {
                new MultiConditionNode(name,ifcond,whilecond),
                node
            });
        }
    }
}

