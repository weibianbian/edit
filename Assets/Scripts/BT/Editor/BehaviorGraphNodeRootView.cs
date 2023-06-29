﻿using UnityEditor.Experimental.GraphView;
namespace BT.Editor
{
    public class BehaviorGraphNodeRootView : BehaviorGraphNodeView
    {
        protected override void InitializePorts()
        {
            var listener = owner.connectorListener;
            AddPort(Direction.Output, false, listener);
        }
    }
    public class BehaviorGraphNodeActionView : BehaviorGraphNodeView
    {
        protected override void InitializePorts()
        {
            var listener = owner.connectorListener;
            AddPort(Direction.Input, false, listener);
        }
    }
}

