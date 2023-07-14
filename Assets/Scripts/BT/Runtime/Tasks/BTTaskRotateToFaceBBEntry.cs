using RailShootGame;

namespace BT.Runtime
{
    [System.Serializable, TreeNodeMenuItem("BT/Task/TaskRotateToFaceBBEntry")]
    public class BTTaskRotateToFaceBBEntry : BTTaskBlackboardBase
    {
        public BTTaskRotateToFaceBBEntry()
        {
            nodeName = "Rotate to face BB entry";
            blackboardKey.AddObjectFilter(this, "", typeof(Actor));
            blackboardKey.AddStringFilter(this, "");
        }
    }
}
