using BT.Runtime;

namespace BT.Graph
{
    [System.Serializable, TreeNodeMenuItem("BT/Action/PlayAnimation")]
    public class PlayAnimationGraphNode : ActionGraphNode
    {
        public PlayAnimationGraphNode()
        {
            classData = typeof(BTPlayAnimationAction);
        }
        protected override void OnVisit()
        {
            base.OnVisit();
        }
    }
}
