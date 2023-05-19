using System;

namespace CopyBT
{
    public class ActionNode : BehaviourNode
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
    public class Leash : BehaviourNode
    {
        public Leash(string name) : base(name)
        {
        }
    }
}

