using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
namespace BT.Editor
{
    public class BaseEdgeConnectorListener : IEdgeConnectorListener
    {
        public readonly BehaviorTreeGraphView graphView;
        Dictionary<Edge, NodePortView> edgeInputPorts = new Dictionary<Edge, NodePortView>();
        Dictionary<Edge, NodePortView> edgeOutputPorts = new Dictionary<Edge, NodePortView>();
        public BaseEdgeConnectorListener(BehaviorTreeGraphView graphView)
        {
            this.graphView = graphView;
        }
        public void OnDrop(GraphView graphView, Edge edge)
        {
            var edgeView = edge as EdgeView;
            bool wasOnTheSamePort = false;

            if (edgeView?.input == null || edgeView?.output == null)
                return;

            //If the edge was moved to another port
            if (edgeView.isConnected)
            {
                if (edgeInputPorts.ContainsKey(edge) && edgeOutputPorts.ContainsKey(edge))
                    if (edgeInputPorts[edge] == edge.input && edgeOutputPorts[edge] == edge.output)
                        wasOnTheSamePort = true;

                //if (!wasOnTheSamePort)
                //    this.graphView.Disconnect(edgeView);
            }

            if (edgeView.input.node == null || edgeView.output.node == null)
                return;

            edgeInputPorts[edge] = edge.input as NodePortView;
            edgeOutputPorts[edge] = edge.output as NodePortView;
            try
            {
                //this.graphView.RegisterCompleteObjectUndo("Connected " + edgeView.input.node.name + " and " + edgeView.output.node.name);
                if (!this.graphView.Connect(edge as EdgeView, autoDisconnectInputs: !wasOnTheSamePort))
                {

                }
                    //this.graphView.Disconnect(edge as EdgeView);
            }
            catch (System.Exception)
            {
                //this.graphView.Disconnect(edge as EdgeView);
            }
        }

        public void OnDropOutsidePort(Edge edge, Vector2 position)
        {
            //this.graphView.RegisterCompleteObjectUndo("Disconnect edge");

            //If the edge was already existing, remove it
            if (!edge.isGhostEdge)
                //graphView.Disconnect(edge as EdgeView);

            // when on of the port is null, then the edge was created and dropped outside of a port
            if (edge.input == null || edge.output == null)
                ShowNodeCreationMenuFromEdge(edge as EdgeView, position);
        }
        void ShowNodeCreationMenuFromEdge(EdgeView edgeView, Vector2 position)
        {
            graphView.createNodeMenu.Initialize(graphView, EditorWindow.focusedWindow, edgeView);
            SearchWindow.Open(new SearchWindowContext(position + EditorWindow.focusedWindow.position.position), graphView.createNodeMenu);
        }
    }
}

