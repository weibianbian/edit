using System.Collections.Generic;

namespace BT.Runtime
{
    public class BehaviorTree
    {
        public BTCompositeNode rootNode = null;
        public BTBlackboardData blackboardData = new BTBlackboardData();

        public BehaviorTree()
        {
            blackboardData = new BTBlackboardData();
        }
    }
}
