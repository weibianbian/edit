using Unity.VisualScripting;

namespace BT.Runtime
{
    public class BTTaskBlackboardBase : BTTaskNode
    {
        [EditAnywhere]
        public BlackboardKeySelector blackboardKey;
        public BTTaskBlackboardBase()
        {
            nodeName = "BlackboardBase";
        }
        public override void InitializeFromAsset(BehaviorTree asset)
        {
            base.InitializeFromAsset(asset);
            BTBlackboardData bbAsset = GetBlackboardAsset();
            if (bbAsset != null)
            {
                blackboardKey.ResolveSelectedKey(bbAsset);
            }
        }
    }
}
