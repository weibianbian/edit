using Unity.VisualScripting;

namespace BT.Runtime
{
    public class BlackboardKeySelector
    {
        public string selectedKeyName;
        public BlackboardKeyTypeBase selectdKeyType;
        protected int selectedKeyID=-1;
        public void ResolveSelectedKey(BTBlackboardData bbAsset)
        {
            if (!string.IsNullOrEmpty(selectedKeyName))
            {
                selectedKeyID = bbAsset.GetKeyID(selectedKeyName);
                selectdKeyType = bbAsset.GetKeyType(selectedKeyID);
            }
        }
    }
    public class BTTaskBlackboardBase : BTTaskNode
    {
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
