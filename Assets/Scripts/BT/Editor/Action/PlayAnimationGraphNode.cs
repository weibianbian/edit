using BT.Runtime;
using GraphProcessor;

namespace BT.Editor
{
    [System.Serializable, NodeMenuItem("BT/Action/PlayAnimation")]
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
