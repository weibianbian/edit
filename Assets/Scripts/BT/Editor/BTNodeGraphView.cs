using BehaviorTree.Runtime;
using GraphProcessor;
using UnityEditor;
using UnityEngine;

namespace BehaviorTree.Editor
{
    [NodeCustomEditor(typeof(BTNode))]
    public class BTNodeGraphView : BaseNodeView
    {
        public override void Enable()
        {
            base.Enable();
           
        }
        public override void OnSelected()
        {
            base.OnSelected();

        }
        protected override void DrawDefaultInspector(bool fromInspector = false)
        {
            Debug.Log("override DrawDefaultInspector");
            this.expanded = false ;
        }
    }
}

