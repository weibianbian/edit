using System;

namespace CopyBT
{
    public class ActionNode : CopyBTBehaviourNode
    {
        public Action action;
        public ActionNode(string name, Action action) : base(name)
        {
            this.action = action;   
        }
        public override void Visit()
        {
            action.Invoke();
            status = ENodeStatus.SUCCESS;
        }
    }
    public class Leash : CopyBTBehaviourNode
    {
        public Leash(string name) : base(name)
        {
        }
    }
}

