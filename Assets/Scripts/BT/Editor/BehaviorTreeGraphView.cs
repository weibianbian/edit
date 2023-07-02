﻿using BT.Runtime;
using Codice.Client.BaseCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BT.Editor
{
    public class BehaviorTreeGraphView : GraphView
    {
        public BaseEdgeConnectorListener connectorListener;
        public BTCreateNodeMenuWindow createNodeMenu;
        EditorWindow window;
        public BehaviorTree treeAsset;
        public event Action initialized;
        public List<EdgeView> edgeViews = new List<EdgeView>();
        public List<BehaviorGraphNodeView> nodeViews = new List<BehaviorGraphNodeView>();
        public BehaviorTreeGraphView(EditorWindow window) : base()
        {
            this.window = window;
            graphViewChanged = GraphViewChangedCallback;
            InitializeManipulators();
            //实现放大或者缩小
            SetupZoom(0.05f, 2f);

            GridBackground gridBackground = new GridBackground();
            Insert(0, gridBackground);
            createNodeMenu = ScriptableObject.CreateInstance<BTCreateNodeMenuWindow>();
            createNodeMenu.Initialize(this, window);
            //this.StretchToParentSize();
        }
        protected virtual void InitializeManipulators()
        {
            ///添加拖拽
            this.AddManipulator(new SelectionDragger());
            ///添加框选
            this.AddManipulator(new RectangleSelector());
            //添加点击选择
            this.AddManipulator(new ClickSelector());
            //添加区域选择
            this.AddManipulator(new ContentDragger());
        }
        public void Initialize()
        {
            connectorListener = CreateEdgeConnectorListener();

            InitializeGraphView();

            initialized?.Invoke();

            BTNodeProvider.LoadGraph();
        }
        void InitializeGraphView()
        {
            nodeCreationRequest += OpenSearchWindow;
        }
        void OpenSearchWindow(NodeCreationContext c)
        {
            if (EditorWindow.focusedWindow == window)
            {
                SearchWindow.Open(new SearchWindowContext(c.screenMousePosition), createNodeMenu);
            }
        }
        GraphViewChange GraphViewChangedCallback(GraphViewChange changes)
        {
            if (changes.elementsToRemove != null)
            {
                //RegisterCompleteObjectUndo("Remove Graph Elements");

                // Destroy priority of objects
                // We need nodes to be destroyed first because we can have a destroy operation that uses node connections
                changes.elementsToRemove.Sort((e1, e2) =>
                {
                    int GetPriority(GraphElement e)
                    {
                        if (e is BehaviorGraphNodeView)
                            return 0;
                        else
                            return 1;
                    }
                    return GetPriority(e1).CompareTo(GetPriority(e2));
                });

                //Handle ourselves the edge and node remove
                changes.elementsToRemove.RemoveAll(e =>
                {

                    switch (e)
                    {
                        case EdgeView edge:
                            Disconnect(edge);
                            return true;
                        case BehaviorGraphNodeView nodeView:
                            // For vertical nodes, we need to delete them ourselves as it's not handled by GraphView
                            if (nodeView.inputPortView != null)
                            {
                                foreach (var edge in nodeView.inputPortView.GetEdges().ToList())
                                    Disconnect(edge);
                            }
                            if (nodeView.outputPortView != null)
                            {
                                foreach (var edge in nodeView.outputPortView.GetEdges().ToList())
                                    Disconnect(edge);
                            }
                            //nodeView.OnRemoved();
                            RemoveElement(nodeView);
                            return true;
                        //case GroupView group:
                        //    graph.RemoveGroup(group.group);
                        //    UpdateSerializedProperties();
                        //    RemoveElement(group);
                        //    return true;
                    }

                    return false;
                });
            }

            return changes;
        }
        public void Disconnect(EdgeView e, bool refreshPorts = true)
        {
            DisconnectView(e, refreshPorts);

            //UpdateComputeOrder();
        }
        public IEnumerable<(string path, Type type)> FilterCreateNodeMenuEntries()
        {
            foreach (var nodeMenuItem in BTNodeProvider.GetNodeMenuEntries())
                yield return nodeMenuItem;
        }
        public BehaviorGraphNodeView AddNode(Type nodeType, Type nodeViewType)
        {
            BehaviorGraphNodeView nodeView = Activator.CreateInstance(nodeViewType) as BehaviorGraphNodeView;
            nodeView.owner = this;
            nodeView.classData = new GraphNodeClassData();
            nodeView.classData.classType = nodeType;
            AddElement(nodeView);
            nodeViews.Add(nodeView);
            return nodeView;
        }
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();

            compatiblePorts.AddRange(ports.ToList().Where(p =>
            {
                var portView = p as NodePortView;

                if (portView.owner == (startPort as NodePortView).owner)
                    return false;

                if (p.direction == startPort.direction)
                    return false;

                //Check for type assignability
                if (!BTTypeUtils.TypesAreConnectable(startPort.portType, p.portType))
                    return false;

                //Check if the edge already exists
                if (portView.GetEdges().Any(e=>EEE(e, startPort)))
                    return false;

                return true;
            }));

            return compatiblePorts;
        }
        private  bool EEE(EdgeView e, Port startPort)
        {
          return  e.input == startPort || e.output == startPort;
        }
        protected virtual BaseEdgeConnectorListener CreateEdgeConnectorListener()
         => new BaseEdgeConnectorListener(this);
        public bool Connect(NodePortView inputPortView, NodePortView outputPortView, bool autoDisconnectInputs = true)
        {
            // Checks that the node we are connecting still exists
            if (inputPortView.owner.parent == null || outputPortView.owner.parent == null)
                return false;

            var edgeView = new EdgeView();
            edgeView.input = inputPortView;
            edgeView.output = outputPortView;


            return Connect(edgeView);
        }
        public bool Connect(EdgeView e, bool autoDisconnectInputs = true)
        {
            if (!CanConnectEdge(e, autoDisconnectInputs))
                return false;

            var inputPortView = e.input as NodePortView;
            var outputPortView = e.output as NodePortView;
            var inputNodeView = inputPortView.node as BehaviorGraphNodeView;
            var outputNodeView = outputPortView.node as BehaviorGraphNodeView;
            var inputPort = inputNodeView.inputPortView;
            var outputPort = outputNodeView.outputPortView;

            ConnectView(e, autoDisconnectInputs);

            //UpdateComputeOrder();

            return true;
        }
        public bool CanConnectEdge(EdgeView e, bool autoDisconnectInputs = true)
        {
            if (e.input == null || e.output == null)
                return false;

            var inputPortView = e.input as NodePortView;
            var outputPortView = e.output as NodePortView;
            var inputNodeView = inputPortView.node as BehaviorGraphNodeView;
            var outputNodeView = outputPortView.node as BehaviorGraphNodeView;

            if (inputNodeView == null || outputNodeView == null)
            {
                Debug.LogError("Connect aborted !");
                return false;
            }

            return true;
        }
        public bool ConnectView(EdgeView e, bool autoDisconnectInputs = true)
        {
            if (!CanConnectEdge(e, autoDisconnectInputs))
                return false;

            var inputPortView = e.input as NodePortView;
            var outputPortView = e.output as NodePortView;
            var inputNodeView = inputPortView.node as BehaviorGraphNodeView;
            var outputNodeView = outputPortView.node as BehaviorGraphNodeView;

            //If the input port does not support multi-connection, we remove them
            if (autoDisconnectInputs && !(e.input as NodePortView).acceptMultipleEdges)
            {
                foreach (var edge in edgeViews.Where(ev => ev.input == e.input).ToList())
                {
                    // TODO: do not disconnect them if the connected port is the same than the old connected
                    DisconnectView(edge);
                }
            }
            // same for the output port:
            if (autoDisconnectInputs && !(e.output as NodePortView).acceptMultipleEdges)
            {
                foreach (var edge in edgeViews.Where(ev => ev.output == e.output).ToList())
                {
                    // TODO: do not disconnect them if the connected port is the same than the old connected
                    DisconnectView(edge);
                }
            }

            AddElement(e);

            e.input.Connect(e);
            e.output.Connect(e);

            // If the input port have been removed by the custom port behavior
            // we try to find if it's still here
            if (e.input == null)
                e.input = inputNodeView.inputPortView;
            if (e.output == null)
                e.output = inputNodeView.inputPortView;

            edgeViews.Add(e);

            inputNodeView.RefreshPorts();
            outputNodeView.RefreshPorts();

            // In certain cases the edge color is wrong so we patch it
            schedule.Execute(() =>
            {
                e.UpdateEdgeControl();
            }).ExecuteLater(1);

            e.isConnected = true;

            return true;
        }
        public void DisconnectView(EdgeView e, bool refreshPorts = true)
        {
            if (e == null)
                return;

            RemoveElement(e);

            if (e?.input?.node is BehaviorGraphNodeView inputNodeView)
            {
                e.input.Disconnect(e);
                if (refreshPorts)
                    inputNodeView.RefreshPorts();
            }
            if (e?.output?.node is BehaviorGraphNodeView outputNodeView)
            {
                e.output.Disconnect(e);
                if (refreshPorts)
                    outputNodeView.RefreshPorts();
            }

            edgeViews.Remove(e);
        }
    }
}

