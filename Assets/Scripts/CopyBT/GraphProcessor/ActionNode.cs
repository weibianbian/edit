using CopyBT;
using GraphProcessor;
using System;
using UnityEngine;

namespace BT.GraphProcessor
{
    [System.Serializable]
    public abstract class ActionNode : BehaviourNode
    {
        [Input(name = "", allowMultiple = false), Vertical]
        public BehaviourNode input;
        public Action action;
        public Blackboard Blackboard
        {
            get { return ownerTreeManager.blackboard; }
        }
        public override Color color =>new Color(0.4f, 0.8f, 0.4f);
        protected override void OnVisit()
        {
            action?.Invoke();
            status = ENodeStatus.SUCCESS;
        }
    }
}
