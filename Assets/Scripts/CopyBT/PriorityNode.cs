using System;
using System.Collections.Generic;
using UnityEngine;

namespace CopyBT
{
    public class EventNode : BehaviourNode
    {
        public float priority = 0;
        public EventNode(string eventName, BehaviourNode child, float priority) : base(eventName, new List<BehaviourNode>() { child })
        {
            this.priority = priority;

        }
    }
    public class PriorityNode : BehaviourNode
    {
        public float period = 0;
        public float lastTime = -1;
        public PriorityNode(List<BehaviourNode> children, float period = 1) : base("Priority", children)
        {
            this.period = period;
            lastTime = -1;
        }
        public override void Reset()
        {
            base.Reset();
            idx = -1;
        }
        public override void Visit()
        {
            float time = DateTime.Now.Millisecond;
            bool do_eval = (lastTime == -1) || (period == -1) || (lastTime + period) < time;
            if (do_eval)
            {
                EventNode oldEvent = null;
                if (idx != -1 && children[idx] is EventNode)
                {
                    oldEvent = children[idx] as EventNode;
                }
                lastTime = time;
                bool found = false;
                for (int i = 0; i < children.Count; i++)
                {
                    BehaviourNode child = children[i];
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
                BehaviourNode child = children[idx];
                if (child.status == ENodeStatus.RUNNING)
                {
                    child.Visit();
                    status = child.status;
                    if (status != ENodeStatus.RUNNING)
                    {
                        lastTime = -1;
                    }
                }
            }
        }
    }
    public class Follow : BehaviourNode
    {
        public Follow(string name) : base("Follow")
        {
        }
    }
}

