using CopyBT;
using GraphProcessor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorTree.Editor
{
    [NodeCustomEditor(typeof(BT.GraphProcessor.BehaviourNode))]
    public class BehaviourNodeView : BaseNodeView
    {
        Label returnLabel;
        private bool isRuned;//节点运行过。
        public override void Enable()
        {
            base.Enable();
            returnLabel = new Label();
            returnLabel.style.color = Color.black;
            returnLabel.style.fontSize = 14;
            returnLabel.style.alignSelf = new StyleEnum<Align>(Align.Stretch);
            SetNodeRunningColor();
            SetLineColorByEnable();
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            RegisterCallback<GeometryChangedEvent>(OnGeometryChangedEvent);
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
            if (nodeTarget is BT.GraphProcessor.BehaviourNode node)
            {
                node.onVisit = SetRunningState;
            }
        }
        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            BT.GraphProcessor.BehaviourNode node = nodeTarget as BT.GraphProcessor.BehaviourNode;
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                Add(returnLabel);
                SetRunningState();
            }
            else if (state == PlayModeStateChange.ExitingPlayMode)
            {
                Remove(returnLabel);
                if (isRuned)
                {
                    isRuned = false;
                    schedule.Execute(() =>
                    {
                        SetHighlightColor(Color.clear);
                        returnLabel.text = "";
                        if (inputPortViews.Count > 0)
                        {
                            var edges = inputPortViews[0].GetEdges();
                            if (edges.Count > 0)
                            {
                                edges[0].output.portColor = Color.white;
                                SetLineColorByEnable();
                            }
                        }
                        MarkDirtyRepaint();
                    }).ExecuteLater(50);
                }
            }
        }
        private void SetLineColorByEnable()
        {
            if (inputPortViews.Count > 0)
            {
                var edges = inputPortViews[0]?.GetEdges();
                if (edges?.Count > 0)
                {
                    Color color = Color.white;
                    //edges[0].output.portColor = color;
                    edges[0].input.portColor = color;
                    edges[0].OnSelected();
                }
            }
        }
        public void SetRunningState()
        {

            if (nodeTarget is BT.GraphProcessor.BehaviourNode node)
            {
                isRuned = true;
                ENodeStatus taskStatus = node.lastResult;
                Color runColor = Color.clear;
                switch (taskStatus)
                {
                    case ENodeStatus.SUCCESS:
                        runColor = new Color(0f, 1f, 0f, 1);
                        returnLabel.text = "✔ Success";
                        break;
                    case ENodeStatus.FAILED:
                        runColor = new Color(1f, 0f, 0f, 1);
                        returnLabel.text = "✘ Failure";
                        break;
                    case ENodeStatus.READY:
                        runColor = new Color(0.5f, 0.5f, 0.5f, 1);
                        returnLabel.text = "○ Ready";
                        break;
                    case ENodeStatus.RUNNING:
                        runColor = new Color(1f, 1f, 0f, 1);
                        returnLabel.text = "✈ Running";
                        break;
                }
                runColor.a = UnityEngine.Random.Range(0.6f, 1f);
                SetHighlightColor(runColor);

                schedule.Execute(() =>
                {
                    if (EditorApplication.isPlaying)
                    {
                        runColor.a = 0.3f;
                        SetHighlightColor(runColor);
                    }
                }).ExecuteLater(500);
                for (int i = 0; i < inputPortViews.Count; i++)
                {
                    List<EdgeView> edges = inputPortViews[i].GetEdges();
                    for (int n = 0; n < edges.Count; n++)
                    {
                        EdgeView edge = edges[n];
                        runColor.a = 1;
                        Color rColor = runColor;
                        edge.output.portColor = rColor;
                        edge.input.portColor = rColor;
                        edge.OnSelected();
                        schedule.Execute(() =>
                        {
                            if (EditorApplication.isPlaying)
                            {
                                Color color = new Color(1, 1, 1, 0.2f);
                                edges[0].output.portColor = color;
                                edges[0].input.portColor = color;
                                edges[0].OnSelected();
                            }
                        }).ExecuteLater(500);
                    }
                }
            }
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
            if (owner == null)
            {
                return;
            }
            //UnityEngine.Debug.Log(owner.GetPinnedElementStatus<NodeInspectorView>());
            //if (owner.GetPinnedElementStatus<NodeInspectorView>()== UnityEngine.UIElements.DropdownMenuAction.Status.Normal)
            //{
            //    //owner.pinnedElements.
            //}
        }
        public override void OnUnselected()
        {
            base.OnUnselected();
            if (owner == null)
            {
                return;
            }
            //if (owner.GetPinnedElementStatus<NodeInspectorView>() != UnityEngine.UIElements.DropdownMenuAction.Status.Hidden)
            //{
            //    owner.ToggleView<NodeInspectorView>();
            //}
        }
        protected override void DrawDefaultInspector(bool fromInspector = false)
        {
            this.expanded = false;
        }
    }
}

