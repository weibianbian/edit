using UnityEditor.Experimental.GraphView;

namespace BT.Editor
{
    public class BehaviorGraphNodeActionView : BehaviorGraphNodeView
    {
        protected override void InitializePorts()
        {
            var listener = owner.connectorListener;
            AddPort(Direction.Input, false, listener);
        }
        public override string GetNodeTitile()
        {
            if (nodeInstance != null)
            {
                return nodeInstance.nodeName;
            }
            return "Unknown";
        }
    }
}

