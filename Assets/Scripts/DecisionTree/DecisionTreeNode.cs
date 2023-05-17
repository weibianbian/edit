using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DecisionTree.Runtime
{
    public class DecisionTest
    {
        Decision root;

        public void Init()
        {
            root = new FloatDecision();
            root.level = 0;

            FloatDecision decision=new FloatDecision();
            decision.level = 1;
            decision.trueNode = new DecisionTreeNode();
            decision.falseNode = new DecisionTreeNode();

            decision.trueNode.child=new FloatDecision();

            root.child = decision;

            root.MakeDecision();
        }
    }
    public class DecisionTreeNode
    {
        public int level = 0;
        public   DecisionTreeNode child=null;
        public virtual DecisionTreeNode MakeDecision()
        {
            return child.MakeDecision();
        }
    }
    public class Action : DecisionTreeNode
    {
        public override DecisionTreeNode MakeDecision()
        {
            return this;
        }
    }
    public class Decision : DecisionTreeNode
    {
        public DecisionTreeNode trueNode;
        public DecisionTreeNode falseNode;

        public virtual DecisionTreeNode GetBranch()
        {
            return trueNode;
        }
        public override DecisionTreeNode MakeDecision()
        {
            DecisionTreeNode branch = GetBranch();
            return branch.MakeDecision();
        }
    }
    public class FloatDecision : Decision
    {
        public override DecisionTreeNode GetBranch()
        {
            if (true)
            {
                return trueNode;
            }
            return falseNode;
        }
    }
}

