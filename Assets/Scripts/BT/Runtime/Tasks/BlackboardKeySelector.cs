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
}
