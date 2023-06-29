﻿using BT.Runtime;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using NodeView = UnityEditor.Experimental.GraphView.Node;
using Status = UnityEngine.UIElements.DropdownMenuAction.Status;
namespace BT.Editor
{
    public class BehaviorGraphNodeRootView : BehaviorGraphNodeView
    {
        protected override void InitializePorts()
        {
            var listener = owner.connectorListener;
            AddPort(Direction.Output, listener);
        }
    }
    public class GraphNodeClassData
    {
        public Type classType;

    }
    public class BehaviorGraphNodeView : NodeView
    {
        public BehaviorTreeGraphView owner { set; get; }
        Label returnLabel;
        private bool isRuned;//节点运行过。
        public BTNode nodeInstance;
        public GraphNodeClassData classData;
        public VisualElement controlsContainer;
        protected VisualElement debugContainer;
        protected VisualElement rightTitleContainer;
        protected VisualElement topPortContainer;
        protected VisualElement bottomPortContainer;
        private VisualElement inputContainerElement;

        public List<NodePortView> inputPortViews = new List<NodePortView>();
        public List<NodePortView> outputPortViews = new List<NodePortView>();

        readonly string baseNodeStyle = "GraphStyles/BaseNodeView";
        public void Initialize()
        {
            styleSheets.Add(Resources.Load<StyleSheet>(baseNodeStyle));
            InitializeView();
            InitializePorts();
            UpdateTitle();
        }
        void InitializeView()
        {
            controlsContainer = new VisualElement { name = "controls" };
            controlsContainer.AddToClassList("NodeControls");
            mainContainer.Add(controlsContainer);
            rightTitleContainer = new VisualElement { name = "RightTitleContainer" };
            titleContainer.Add(rightTitleContainer);

            topPortContainer = new VisualElement { name = "TopPortContainer" };
            this.Insert(0, topPortContainer);

            bottomPortContainer = new VisualElement { name = "BottomPortContainer" };
            this.Add(bottomPortContainer);
        }
        protected virtual void InitializePorts()
        {
            var listener = owner.connectorListener;
            AddPort(Direction.Input, listener);
            AddPort(Direction.Output, listener);
        }
        public void AddPort(Direction direction, BaseEdgeConnectorListener listener)
        {
            NodePortView p = CreatePortView(direction, listener);


            if (p.direction == Direction.Input)
            {
                inputPortViews.Add(p);
                topPortContainer.Add(p);
            }
            else
            {
                outputPortViews.Add(p);
                bottomPortContainer.Add(p);
            }
            p.Initialize(this, "@@@sdfsdfsdfsd");
        }
        protected virtual NodePortView CreatePortView(Direction direction, BaseEdgeConnectorListener listener)
           => NodePortView.CreatePortView(direction, listener);
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Open Node Script", (e) => { }, OpenNodeScriptStatus);
            evt.menu.AppendAction("Open Node View Script", (e) => { }, OpenNodeScriptStatus);
            evt.menu.AppendAction("Debug", (e) => { }, OpenNodeScriptStatus);
        }
        Status OpenNodeScriptStatus(DropdownMenuAction action)
        {
            //if (TreeNodeProvider.GetNodeScript(nodeTarget.GetType()) != null)
            //    return Status.Normal;
            return Status.Disabled;
        }
        public void PostPlaceNewNode()
        {
            if (nodeInstance == null)
            {
                nodeInstance = Activator.CreateInstance(classData.classType) as BTNode;
            }
            Initialize();
        }
        void UpdateTitle()
        {
            title = nodeInstance.nodeName;
        }
        public void Enable()
        {
            returnLabel = new Label();
            returnLabel.style.color = Color.black;
            returnLabel.style.fontSize = 14;
            returnLabel.style.alignSelf = new StyleEnum<Align>(Align.Stretch);
            SetNodeRunningColor();
            SetLineColorByEnable();
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }
        protected void OnGeometryChangedEvent(GeometryChangedEvent evt)
        {
            //首次进来更新线的颜色
            SetLineColorByEnable();
        }
        private void SetNodeRunningColor()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            if (EditorApplication.isPlaying)
            {
                //如果打开Graph编辑器时Unity在Play，那么主动设置一下运行时效果。
                OnPlayModeStateChanged(PlayModeStateChange.EnteredPlayMode);
            }
            //if (nodeTarget is BT.Editor.BehaviourGraphNode node)
            //{
            //    node.onVisit = SetRunningState;
            //}
        }
        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            //BT.Editor.BehaviourGraphNode node = nodeTarget as BT.Editor.BehaviourGraphNode;
            //if (state == PlayModeStateChange.EnteredPlayMode)
            //{
            //    Add(returnLabel);
            //    SetRunningState();
            //}
            //else if (state == PlayModeStateChange.ExitingPlayMode)
            //{
            //    Remove(returnLabel);
            //    if (isRuned)
            //    {
            //        isRuned = false;
            //        schedule.Execute(() =>
            //        {
            //            SetHighlightColor(Color.clear);
            //            returnLabel.text = "";
            //            if (inputPortViews.Count > 0)
            //            {
            //                var edges = inputPortViews[0].GetEdges();
            //                if (edges.Count > 0)
            //                {
            //                    edges[0].output.portColor = Color.white;
            //                    SetLineColorByEnable();
            //                }
            //            }
            //            MarkDirtyRepaint();
            //        }).ExecuteLater(50);
            //    }
            //}
        }
        private void SetLineColorByEnable()
        {
            //if (inputPortViews.Count > 0)
            //{
            //    var edges = inputPortViews[0]?.GetEdges();
            //    if (edges?.Count > 0)
            //    {
            //        Color color = Color.white;
            //        //edges[0].output.portColor = color;
            //        edges[0].input.portColor = color;
            //        edges[0].OnSelected();
            //    }
            //}
        }
        public void SetRunningState()
        {

            //if (nodeTarget is BehaviourGraphNode node)
            //{
            //    isRuned = true;
            //    ENodeStatus taskStatus = node.lastResult;
            //    Color runColor = Color.clear;
            //    switch (taskStatus)
            //    {
            //        case ENodeStatus.SUCCESS:
            //            runColor = new Color(0f, 1f, 0f, 1);
            //            returnLabel.text = "✔ Success";
            //            break;
            //        case ENodeStatus.FAILED:
            //            runColor = new Color(1f, 0f, 0f, 1);
            //            returnLabel.text = "✘ Failure";
            //            break;
            //        case ENodeStatus.READY:
            //            runColor = new Color(0.5f, 0.5f, 0.5f, 1);
            //            returnLabel.text = "○ Ready";
            //            break;
            //        case ENodeStatus.RUNNING:
            //            runColor = new Color(1f, 1f, 0f, 1);
            //            returnLabel.text = "✈ Running";
            //            break;
            //    }
            //    runColor.a = UnityEngine.Random.Range(0.6f, 1f);
            //    SetHighlightColor(runColor);

            //    schedule.Execute(() =>
            //    {
            //        if (EditorApplication.isPlaying)
            //        {
            //            runColor.a = 0.3f;
            //            SetHighlightColor(runColor);
            //        }
            //    }).ExecuteLater(500);
            //    for (int i = 0; i < inputPortViews.Count; i++)
            //    {
            //        List<EdgeView> edges = inputPortViews[i].GetEdges();
            //        for (int n = 0; n < edges.Count; n++)
            //        {
            //            EdgeView edge = edges[n];
            //            runColor.a = 1;
            //            Color rColor = runColor;
            //            edge.output.portColor = rColor;
            //            edge.input.portColor = rColor;
            //            edge.OnSelected();
            //            schedule.Execute(() =>
            //            {
            //                if (EditorApplication.isPlaying)
            //                {
            //                    Color color = new Color(1, 1, 1, 0.2f);
            //                    edges[0].output.portColor = color;
            //                    edges[0].input.portColor = color;
            //                    edges[0].OnSelected();
            //                }
            //            }).ExecuteLater(500);
            //        }
            //    }
            //}
        }
        public void SetHighlightColor(Color color)
        {
            returnLabel.style.color = color;
            color.a = 0.1f;
            returnLabel.style.backgroundColor = color;
        }
        public override void OnSelected()
        {
            base.OnSelected();
            //if (owner == null)
            //{
            //    return;
            //}
            //UnityEngine.Debug.Log(owner.GetPinnedElementStatus<NodeInspectorView>());
            //if (owner.GetPinnedElementStatus<NodeInspectorView>()== UnityEngine.UIElements.DropdownMenuAction.Status.Normal)
            //{
            //    //owner.pinnedElements.
            //}
        }
        public override void OnUnselected()
        {
            base.OnUnselected();
            //if (owner == null)
            //{
            //    return;
            //}
            //if (owner.GetPinnedElementStatus<NodeInspectorView>() != UnityEngine.UIElements.DropdownMenuAction.Status.Hidden)
            //{
            //    owner.ToggleView<NodeInspectorView>();
            //}
        }
        //protected override void DrawDefaultInspector(bool fromInspector = false)
        //{
        //    this.expanded = false;
        //}
    }
}

