using BT.Runtime;

namespace BT.Graph
{
    public class EventGraphNode : BehaviorGraphNode
    {
        public float priority = 0;
        bool triggered = false;
        public void OnEvent()
        {
            if (status == ENodeStatus.RUNNING)
            {
                ChildAtIndex(0).Reset();
            }
            triggered = true;
            //self.inst.brain:ForceUpdate()

        }
    }
}
