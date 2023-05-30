using CopyBT;
using GraphProcessor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorTree.Editor
{
    [NodeCustomEditor(typeof(CopyBT.GraphProcessor.BehaviourNode))]
    public class BehaviourNodeView : BaseNodeView
    {
        Label returnLabel;
        public override void Enable()
        {
            base.Enable();
            CopyBT.GraphProcessor.BehaviourNode node = nodeTarget as CopyBT.GraphProcessor.BehaviourNode;
            returnLabel = new Label();
            debugContainer.Add(returnLabel);
            returnLabel.text = "✔ Success";
            returnLabel.style.color = new Color(0f, 1f, 0f, 1);
            returnLabel.style.fontSize = 22;
            returnLabel.style.alignSelf = new StyleEnum<Align>(Align.Stretch);
            //EnumField accessorSelector = new EnumField(node.accessor);
            //accessorSelector.SetValueWithoutNotify(parameterNode.accessor);
            for (int i = 0; i < outputPortViews.Count; i++)
            {
                List<EdgeView> edgeView = outputPortViews[i].GetEdges();
                edgeView.Sort((e1, e2) => e1.serializedEdge.inputNode.position.x < e2.serializedEdge.inputNode.position.x ? -1 : 1);
            }
            SetRunningState();

            node.onVisit += SetRunningState;

        }
        public void SetRunningState()
        {

            if (nodeTarget is CopyBT.GraphProcessor.BehaviourNode node)
            {
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
                runColor.a = Random.Range(0.6f, 1f);
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

        }
        protected override void DrawDefaultInspector(bool fromInspector = false)
        {
            Debug.Log("override DrawDefaultInspector");
            this.expanded = false;
        }
    }
}

