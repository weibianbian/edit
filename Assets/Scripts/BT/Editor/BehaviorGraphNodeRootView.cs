using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BT.Editor
{
    public class BehaviorGraphNodeRootView : BehaviorGraphNodeView
    {
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
    }
    public class BehaviorGraphNodeActionView : BehaviorGraphNodeView
    {
        protected override void InitializePorts()
        {
            var listener = owner.connectorListener;
            AddPort(Direction.Input, false, listener);
        }
    }
    public class BehaviorGraphNodeCompositeView : BehaviorGraphNodeView
    {
        protected override void InitializePorts()
        {
            var listener = owner.connectorListener;
            AddPort(Direction.Input, false, listener);
            AddPort(Direction.Output, true, listener);
        }
    }
}

