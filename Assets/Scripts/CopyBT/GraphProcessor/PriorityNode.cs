using BehaviorTree.Runtime;
using GraphProcessor;
using System;
using UnityEngine;

namespace CopyBT.GraphProcessor
{
    [System.Serializable, NodeMenuItem("BT/CompositeNode/Priority")]
    public class PriorityNode : BehaviourNode
    {
        [Input(name = "", allowMultiple = false), Vertical]
        public ENodeStatus input;
        [Output("", true), Vertical]
        public ENodeStatus output;
        public static readonly int nil = -1;
        public float period = 0.25f;
        public float lastTime = nil;

        protected override void Enable()
        {
            base.Enable();
            lastTime = nil;
        }
        public override void Reset()
        {
            base.Reset();
            idx = nil;
        }
        public override void Visit()
        {
            float time =Time.realtimeSinceStartup;
            bool do_eval = (lastTime == nil) || (period == nil) || (lastTime + period) < time;
            if (do_eval)
            {
                EventNode oldEvent = null;
                if (IsValidIndex(idx))
                {
                    if (ChildAtIndex(idx) is EventNode)
                    {
                        oldEvent = ChildAtIndex(idx) as EventNode;
                    }
                }
                lastTime = time;
                bool found = false;
                for (int i = 0; i < ChildCount; i++)
                {
                    BehaviourNode child = ChildAtIndex(i);
                    bool should_test_anyway = oldEvent != null && child is EventNode && oldEvent.priority <= (child as EventNode).priority;
                    if (!found || should_test_anyway)
                    {
                        if (child.status == ENodeStatus.FAILED || child.status == ENodeStatus.SUCCESS)
                        {
                            child.Reset();
                        }
                        child.Visit();
                        ENodeStatus cs = child.status;
                        if (cs == ENodeStatus.SUCCESS || cs == ENodeStatus.RUNNING)
                        {
                            if (should_test_anyway && idx != i)
                            {
                                ChildAtIndex(idx).Reset();
                            }
                            status = cs;
                            found = true;
                            idx = i;
                        }
                    }
                    else
                    {
                        child.Reset();
                    }

                }
                if (!found)
                {
                    status = ENodeStatus.FAILED;
                }
            }
            else
            {
                if (IsValidIndex(idx))
                {
                    BehaviourNode child = ChildAtIndex(idx);
                    if (child.status == ENodeStatus.RUNNING)
                    {
                        child.Visit();
                        status = child.status;
                        if (status != ENodeStatus.RUNNING)
                        {
                            lastTime = nil;
                        }
                    }
                }
            }
        }
    }
}
