﻿namespace BT.Runtime
{
    [System.Serializable, TreeNodeMenuItem("BT/Composite/Sequence")]
    public class BTSequenceCompositieNode : BTCompositieNode
    {
        public BTSequenceCompositieNode() {
            nodeName = "序列节点";
        }
        protected override void OnVisit()
        {
            if (status != ENodeStatus.RUNNING)
            {
                idx = 0;
            }
            bool done = false;
            while (idx < ChildCount)
            {
                BTNode child = ChildAtIndex(idx);
                child.Visit();
                if (child.status == ENodeStatus.RUNNING || child.status == ENodeStatus.FAILED)
                {
                    status = child.status;
                    return;
                }
                idx++;
            }
            status = ENodeStatus.SUCCESS;
        }
        public BTSequenceNodeData NP_SequenceNodeData = new BTSequenceNodeData { NodeDes = "序列组合器" };
        public override BTNodeDataBase GetNodeData()
        {
            return NP_SequenceNodeData;
        }
    }
}
