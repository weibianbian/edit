using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace BT.Editor
{
    public class NodePortView : Port
    {
        protected NodePortView(Direction portDirection) : base(Orientation.Vertical, portDirection, Capacity.Multi, typeof(BehaviorGraphNodeView))
        {
        }
        public static NodePortView CreatePortView(Direction direction)
        {
            var pv = new NodePortView(direction);
            pv.AddManipulator(pv.m_EdgeConnector);

            // Force picking in the port label to enlarge the edge creation zone
            var portLabel = pv.Q("type");
            if (portLabel != null)
            {
                portLabel.pickingMode = PickingMode.Position;
                portLabel.style.flexGrow = 1;
            }

            // hide label when the port is vertical
            portLabel.style.display = DisplayStyle.None;

            // Fixup picking mode for vertical top ports
            pv.Q("connector").pickingMode = PickingMode.Position;

            return pv;
        }
    }
}

