using BehaviorTree.Runtime;
using CopyBT.GraphProcessor;
using GraphProcessor;
using UnityEngine;

namespace BehaviorTree.Editor
{
    [NodeCustomEditor(typeof(BehaviourNode))]
    public class BehaviourNodeView : BaseNodeView
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

