using BT.Runtime;
using GraphProcessor;
using System;
using System.Runtime.Remoting;
using UnityEngine;

namespace BT.Editor
{
    [System.Serializable]
    public abstract class BehaviourGraphNode : BaseNode
    {
        [ShowInInspector]
        public BehaviourGraphNode parent;
        public ENodeStatus status = ENodeStatus.READY;
        public ENodeStatus lastResult = ENodeStatus.READY;
        public float nextUpdateTick = 0;
        public int idx = 0;
        public Action onVisit;
        [NonSerialized]
        public BTManager ownerTreeManager;
        public BTNode nodeInstance;
        public Type classData;
        protected override void Enable()
        {
            base.Enable();

        }
        public override void OnNodeCreated()
        {
            onAfterEdgeConnected -= Action;
            onAfterEdgeConnected += Action;
            base.OnNodeCreated();
            PostPlaceNewNode();

        }
        public void PostPlaceNewNode()
        {
            if (nodeInstance==null)
            {
                nodeInstance = Activator.CreateInstance(classData) as BTNode;
            }
        }
        public void Action(SerializableEdge e)
        {
            UnityEngine.Debug.LogError($"{this}    {e.ToString()}");
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

        protected BehaviourGraphNode ChildAtIndex(int index)
        {
            if (outputPorts.Count > 0)
            {
                var edges = outputPorts[0].GetEdges();
                if (edges.Count > index)
                {
                    return edges[index].inputNode as BehaviourGraphNode;
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
        public virtual void DoToParents(Action<BehaviourGraphNode> fn)
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
        public virtual BTNodeDataBase GetNodeData()
        {
            return null;
        }
    }
}
