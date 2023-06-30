using BT.Runtime;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BT.Editor
{
    public class NodePortView : Port
    {
        string userPortStyleFile = "PortViewTypes";
        readonly string portStyle = "GraphStyles/PortView";
        List<EdgeView> edges = new List<EdgeView>();
        public bool acceptMultipleEdges;
        public BehaviorGraphNodeView owner { get; private set; }
        public string fieldName => typeof(BTNode).ToString();
        public Type fieldType => typeof(BTNode);
        protected NodePortView(Direction portDirection,bool allowMultiple) : base(Orientation.Vertical, portDirection, Capacity.Multi, typeof(BehaviorGraphNodeView))
        {
            acceptMultipleEdges = allowMultiple;
            styleSheets.Add(Resources.Load<StyleSheet>(portStyle));
            var userPortStyle = Resources.Load<StyleSheet>(userPortStyleFile);
            if (userPortStyle != null)
            {
                styleSheets.Add(userPortStyle);
            }
            AddToClassList("Vertical");
            UpdatePortSize();
        }
        public static NodePortView CreatePortView(Direction direction, bool allowMultiple, BaseEdgeConnectorListener edgeConnectorListener)
        {
            var pv = new NodePortView(direction, allowMultiple);
            pv.m_EdgeConnector = new BaseEdgeConnector(edgeConnectorListener);
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
        public virtual void Initialize(BehaviorGraphNodeView nodeView, string name)
        {
            this.owner = nodeView;
            AddToClassList("xixixixixi");

            // Correct port type if port accept multiple values (and so is a container)
            if (direction == Direction.Input && acceptMultipleEdges && portType == fieldType) // If the user haven't set a custom field type
            {
                if (fieldType.GetGenericArguments().Length > 0)
                    portType = fieldType.GetGenericArguments()[0];
            }

            if (name != null)
                portName = name;
            visualClass = "Port_" + portType.Name;
            //tooltip = portData.tooltip;
        }
        public override void Connect(Edge edge)
        {
            base.Connect(edge);
            var inputNode = (edge.input as NodePortView).owner;
            var outputNode = (edge.output as NodePortView).owner;
            edges.Add(edge as EdgeView);

            inputNode.OnPortConnected(edge.input as NodePortView);
            outputNode.OnPortConnected(edge.output as NodePortView);
        }
       
        public void UpdatePortSize()
        {
            int size = 8;
            var connector = this.Q("connector");
            var cap = connector.Q("cap");
            connector.style.width = size;
            connector.style.height = size;
            cap.style.width = size - 4;
            cap.style.height = size - 4;

            // Update connected edge sizes:
            edges.ForEach(e => e.UpdateEdgeSize());
        }
        public List<EdgeView> GetEdges()
        {
            return edges;
        }
    }
}

