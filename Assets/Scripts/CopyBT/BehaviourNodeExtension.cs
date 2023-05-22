using System;
using System.Collections.Generic;

namespace CopyBT
{
    public static class BehaviourNodeExtension
    {
        public static BehaviourNode WhileNode(Func<bool> cond, string name, BehaviourNode node)
        {
            return new ParallelNode(new List<BehaviourNode>()
            {
                new ConditionNode(cond),
                node
            });
        }
        public static BehaviourNode IfNode(Func<bool> cond, string name, BehaviourNode node)
        {
            return new SequenceNode(new List<BehaviourNode>()
            {
                new ConditionNode(cond),
                node
            });
        }
        public static BehaviourNode IfThenDoWhileNode(Func<bool> ifcond, Func<bool> whilecond, string name, BehaviourNode node)
        {
            return new ParallelNode(new List<BehaviourNode>()
            {
                new MultiConditionNode(name,ifcond,whilecond),
                node
            });
        }
    }
}

