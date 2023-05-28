using CopyBT;
using GraphProcessor;
using static Unity.VisualScripting.Metadata;

namespace BehaviorTree.Runtime
{
    [System.Serializable]
    public class BTNode : BaseNode
    {
        public ENodeStatus lastResult = ENodeStatus.READY;
        public ENodeStatus status = ENodeStatus.READY;
        public override bool isRenamable => true;
        public override void InitializePorts()
        {
            base.InitializePorts();
            debug = true;
        }
        public virtual void Visit()
        {

        }
        public virtual void Step()
        {

        }
        public virtual void SaveStatus()
        {
            lastResult = status;
        }
    }
}

