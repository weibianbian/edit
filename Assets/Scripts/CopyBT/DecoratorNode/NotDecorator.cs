namespace CopyBT
{
    public class NotDecorator : DecoratorNode
    {
        public NotDecorator(string name, BehaviourNode child) : base("Not", child)
        {
        }
        public override void Visit()
        {
            BehaviourNode child = children[0];
            child.Visit();
            if (child.status == ENodeStatus.SUCCESS)
            {
                status = ENodeStatus.FAILED;
            }
            else if (child.status == ENodeStatus.FAILED)
            {
                status = ENodeStatus.SUCCESS;
            }
            else
            {
                status = child.status;
            }
        }
    }
}

