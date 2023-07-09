using BT.Runtime;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BT.Editor
{
    public class BehaviorGraphNodeRootView : BehaviorGraphNodeView
    {
        public BTBlackboardData blackboardAsset;
        public override Type RuntimeClassType => null;
        protected override void InitializePorts()
        {
            var listener = owner.connectorListener;
            AddPort(Direction.Output, false, listener);
        }
        protected override void SetNodeColor()
        {
            titleContainer.style.borderBottomColor = new StyleColor(Color.cyan);
            titleContainer.style.borderBottomWidth = new StyleFloat(Color.cyan.a > 0 ? 5f : 0f);
        }
        public override string GetNodeTitile()
        {
            return "Root";
        }
        public override void PostPlacedNewNode()
        {
            base.PostPlacedNewNode();
            UpdateBlackboard();
        }
        public void UpdateBlackboard()
        {

        }
    }
}

