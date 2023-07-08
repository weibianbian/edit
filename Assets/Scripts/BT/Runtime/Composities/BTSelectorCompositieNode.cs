namespace BT.Runtime
{
    [System.Serializable, TreeNodeMenuItem("BT/Composite/Selector")]
    public class BTSelectorCompositieNode : BTCompositeNode
    {
        public BTSelectorCompositieNode()
        {
            nodeName = "选择节点";
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
                BTCompositeChild compositeChild = ChildAtIndex(idx);
                BTNode child = compositeChild.childComposite != null ? compositeChild.childComposite : compositeChild.childAction;
                child.Visit();
                if (child.status == ENodeStatus.RUNNING || child.status == ENodeStatus.SUCCESS)
                {
                    status = child.status;
                    return;
                }
                idx++;
            }
            status = ENodeStatus.FAILED;
        }
        public BTSequenceNodeData NP_SequenceNodeData = new BTSequenceNodeData { NodeDes = "序列组合器" };
        public override BTNodeDataBase GetNodeData()
        {
            return NP_SequenceNodeData;
        }
    }
}
