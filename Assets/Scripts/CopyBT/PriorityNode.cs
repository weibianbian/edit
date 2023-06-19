using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CopyBT
{
    public class PriorityNode : CopyBTBehaviourNode
    {
        public int nil = -1;
        public float period = 0;
        public float lastTime = -1;
        public PriorityNode(List<CopyBTBehaviourNode> children, float period = 1) : base("Priority", children)
        {
            this.period = period;
            lastTime = nil;
        }
        public override string DBString()
        {
            float time_till = lastTime + period - DateTime.Now.Millisecond;

            return $"execute {idx}, eval in {time_till}";
        }
        public override void Reset()
        {
            base.Reset();
            idx = nil;
        }
        public override void Visit()
        {
            float time = DateTime.Now.Millisecond;
            bool do_eval = (lastTime == nil) || (period == nil) || (lastTime + period) < time;
            if (do_eval)
            {
                EventNode oldEvent = null;
                if (IsValidIndex(idx))
                {
                    if (children[idx] is EventNode)
                    {
                        oldEvent = children[idx] as EventNode;
                    }
                }
                lastTime = time;
                bool found = false;
                for (int i = 0; i < children.Count; i++)
                {
                    CopyBTBehaviourNode child = children[i];
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
                                children[idx].Reset();
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
                    CopyBTBehaviourNode child = children[idx];
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

