using BT.Runtime;
using System;
using UnityEngine;

namespace BT.Graph
{
    [System.Serializable]
    public abstract class ActionGraphNode : BehaviorGraphNode
    {
        [Input(name = "", allowMultiple = false), Vertical]
        public BehaviorGraphNode input;
        public Action action;
        //public Blackboard Blackboard
        //{
        //    get { return null; }
        //}
        public override Color color =>new Color(0.4f, 0.8f, 0.4f);
        protected override void OnVisit()
        {
            action?.Invoke();
            status = ENodeStatus.SUCCESS;
        }
    }
}
