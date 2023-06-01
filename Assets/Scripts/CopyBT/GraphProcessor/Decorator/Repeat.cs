using CopyBT;
using GraphProcessor;
using UnityEngine;

namespace BT.GraphProcessor
{
    [System.Serializable, NodeMenuItem("BT/Decorator/Repeat")]
    public class Repeat : DecoratorNode
    {
        protected override void OnVisit()
        {
            var child = ChildAtIndex(0);
            child.Visit();
            status = ENodeStatus.RUNNING;
        }
    }
}
