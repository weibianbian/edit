using GraphProcessor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorTree.Editor
{
    //[CustomEditor(typeof(NodeInspectorView))]
    //public class BTNodeInspectorObjectEditor : NodeInspectorObjectEditor
    //{
    //    protected override void OnEnable()
    //    {
    //        base.OnEnable();
    //    }

    //}

    //public class NodeInspectorView : NodeInspectorObject
    //{
    //    private Label subTitleLabel;
    //    public override void UpdateSelectedNodes(HashSet<BaseNodeView> views)
    //    {
    //        base.UpdateSelectedNodes(views);
    //        Debug.LogError("选择了节点");
    //    }

    //    //protected override void Initialize(BaseGraphView graphView)
    //    //{
    //    //    title = "NodeInspector";
    //    //    subTitleLabel = new Label();
    //    //    int onSelectNodeCount = graphView.selection.Count(e => e is BaseNodeView);

    //    //    if (onSelectNodeCount == 1)
    //    //    {
    //    //        graphView.selection.ForEach(e => {
    //    //            if (e is BaseNodeView nodeView)
    //    //            {
    //    //                OnSelectNodeView(graphView,nodeView);
    //    //            }
    //    //        });
    //    //    }
    //    //    else
    //    //    {
    //    //        content.Clear();
    //    //        content.Add(subTitleLabel);
    //    //        SetOnSelectNodeCount(graphView);
    //    //    }
    //    //}
    //    //private void SetOnSelectNodeCount(BaseGraphView graphView)
    //    //{
    //    //    int count = graphView.selection.Count(e => e is BaseNodeView);
    //    //    if (count != 1)
    //    //    {
    //    //        subTitleLabel.text = $"Select a node to view its properties. \nCurrent select node count : {count}.";
    //    //    }
    //    //}
    //    //public void OnSelectNodeView(BaseGraphView graphView,BaseNodeView nodeView)
    //    //{
    //    //    if (graphView.selection.Count > 1)
    //    //    {
    //    //        //选中多个，清空显示。
    //    //        OnNodeUnSelect(nodeView);
    //    //        return;
    //    //    }
    //    //    showPropertyFields.Clear();
    //    //    content.Clear();
    //    //    content.Add(subTitleLabel);
    //    //    var fields = nodeView.nodeTarget.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
    //    //        .Where(f => f.DeclaringType != typeof(BaseNode));
    //    //    fields = nodeView.nodeTarget.OverrideFieldOrder(fields).Reverse();
    //    //    foreach (var field in fields)
    //    //    {
    //    //        bool serializeField = field.GetCustomAttribute(typeof(SerializeField)) != null;
    //    //        if ((!field.IsPublic && !serializeField) || field.IsNotSerialized)
    //    //        {
    //    //            continue;
    //    //        }
    //    //        bool hasInputAttribute = field.GetCustomAttribute(typeof(InputAttribute)) != null;
    //    //        bool hasInputOrOutputAttribute = hasInputAttribute || field.GetCustomAttribute(typeof(OutputAttribute)) != null;
    //    //        //bool showAsDrawer = fromInspector && field.GetCustomAttribute(typeof(ShowAsDrawer)) != null;
    //    //        if (!serializeField && hasInputOrOutputAttribute)// && !showAsDrawer)
    //    //        {
    //    //            continue;
    //    //        }
    //    //        DrawField(field, nodeView);
    //    //    }
    //    //}
    //    //public void OnNodeUnSelect(BaseNodeView nodeView)
    //    //{
    //    //    showPropertyFields.Clear();
    //    //    content.Clear();
    //    //    content.Add(subTitleLabel);
    //    //}
    //    //protected void DrawField(FieldInfo field, BaseNodeView nodeView)
    //    //{
    //    //    string displayName = ObjectNames.NicifyVariableName(field.Name);
    //    //    var inspectorNameAttribute = field.GetCustomAttribute<InspectorNameAttribute>();
    //    //    if (inspectorNameAttribute != null)
    //    //        displayName = inspectorNameAttribute.displayName;

    //    //    var propertyField = new PropertyField(FindSerializedProperty(nodeView, field.Name), displayName);
    //    //    propertyField.Bind(nodeView.owner.serializedGraph);

    //    //    if (propertyField != null)
    //    //    {
    //    //        content.Add(propertyField);
    //    //        propertyField.name = displayName;
    //    //        showPropertyFields.Add(field.Name, propertyField);
    //    //    }
    //    //}
    //    //protected SerializedProperty FindSerializedProperty(BaseNodeView nodeView, string fieldName)
    //    //{
    //    //    int i = nodeView.owner.graph.nodes.FindIndex(n => n == nodeView.nodeTarget);
    //    //    return nodeView.owner.serializedGraph.FindProperty("nodes").GetArrayElementAtIndex(i).FindPropertyRelative(fieldName);
    //    //}
    //}
}

