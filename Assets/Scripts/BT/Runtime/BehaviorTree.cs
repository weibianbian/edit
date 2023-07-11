using System.Collections.Generic;

namespace BT.Runtime
{
    public class BehaviorTree
    {
        public BTCompositeNode rootNode = null;
        public BTBlackboardData blackboardData;

        public BehaviorTree()
        {
            blackboardData = new BTBlackboardData();
        }
    }
}
