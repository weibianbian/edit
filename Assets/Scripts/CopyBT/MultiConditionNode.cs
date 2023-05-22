using System;

namespace CopyBT
{
    public class MultiConditionNode : BehaviourNode
    {
        bool isStart = false;
        bool running = false;
        Func<bool> start;
        Func<bool> co;
        public MultiConditionNode(string name, Func<bool> start, Func<bool> co) : base(name)
        {
            isStart = false;
        }
        public override void Visit()
        {
            if (!isStart)
            {
                running = start();
                isStart = true;
            }
            else
            {
                running = co();
            }
            status = running ? ENodeStatus.SUCCESS : ENodeStatus.FAILED;
        }
    }
}

