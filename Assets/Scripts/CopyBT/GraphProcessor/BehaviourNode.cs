using CopyBT;
using GraphProcessor;
using System;
using UnityEngine;

namespace BT.GraphProcessor
{
    public class Blackboard : ScriptableObject
    {

    }
    [System.Serializable]
    public class BehaviourNode : BaseNode
    {
        [ShowInInspector]
        public BehaviourNode parent;
        public ENodeStatus status = ENodeStatus.READY;
        public ENodeStatus lastResult = ENodeStatus.READY;
        public float nextUpdateTick = 0;
        public int idx = 0;
        public Action onVisit;
        [NonSerialized]
        public BTManager ownerTreeManager;
        protected override void Enable()
        {
            base.Enable();

        }
        public override bool isRenamable => true;
        protected int ChildCount
        {
            get
            {
                if (outputPorts.Count > 0)
                {
                    return outputPorts[0].GetEdges().Count;
                }
                return 0;
            }
        }

        protected BehaviourNode ChildAtIndex(int index)
        {
            if (outputPorts.Count > 0)
            {
                var edges = outputPorts[0].GetEdges();
                if (edges.Count > index)
                {
                    return edges[index].inputNode as BehaviourNode;
                }
            }
            return null;
        }
        public void Visit()
        {
            OnVisit();
            onVisit?.Invoke();
        }
        protected virtual void OnVisit()
        {

        }
        public virtual void DoToParents(Action<BehaviourNode> fn)
        {
            if (parent != null)
            {
                fn(parent);
                parent.DoToParents(fn);
            }
        }
        public virtual void Step()
        {
            if (status != ENodeStatus.RUNNING)
            {
                Reset();
            }
            else
            {
                for (int i = 0; i < ChildCount; i++)
                {
                    ChildAtIndex(i).Step();
                }
            }
        }
        public virtual void Reset()
        {
            if (status != ENodeStatus.READY)
            {
                status = ENodeStatus.READY;
                for (int i = 0; i < ChildCount; i++)
                {
                    ChildAtIndex(i).Reset();
                }
            }
        }
        public void SaveStatus()
        {
            lastResult = status;
            for (int i = 0; i < ChildCount; i++)
            {
                ChildAtIndex(i).SaveStatus();
            }
        }
        public bool IsValidIndex(int idx)
        {
            return idx >= 0 && idx < ChildCount;
        }
    }
}
