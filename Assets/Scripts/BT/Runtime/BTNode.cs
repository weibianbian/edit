using System;
using UnityEngine;

namespace BT.Runtime
{
    [Serializable]
    public class BTNode : ISerializationCallbackReceiver
    {
        public BTCompositeNode parentNode;
        public ENodeStatus status = ENodeStatus.READY;
        public ENodeStatus lastResult = ENodeStatus.READY;
        public float nextUpdateTick = 0;
        public int idx = 0;
        public Action onVisit;

        public string nodeName = "";

        public int executionIndex = 0;
        public int treeDepth = 0;
        public void InitializeNode(BTCompositeNode inParentNode, int inExecutionIndex, int inTreeDepth)
        {
            parentNode = inParentNode;
            executionIndex = inExecutionIndex;
            treeDepth = inTreeDepth;
        }
        public void Visit()
        {
            OnVisit();
            onVisit?.Invoke();
        }
        protected virtual void OnVisit()
        {

        }
        public virtual void DoToParents(Action<BTNode> fn)
        {
            if (parentNode != null)
            {
                fn(parentNode);
                parentNode.DoToParents(fn);
            }
        }
        public virtual void Step()
        {
            if (status != ENodeStatus.RUNNING)
            {
                Reset();
            }
        }
        public virtual void Reset()
        {
            if (status != ENodeStatus.READY)
            {
                status = ENodeStatus.READY;
            }
        }
        public void SaveStatus()
        {
            lastResult = status;
        }
        public virtual BTNodeDataBase GetNodeData()
        {
            return null;
        }

        public virtual void OnBeforeSerialize()
        {
            Debug.Log("OnBeforeSerialize");
        }

        public virtual void OnAfterDeserialize()
        {
            Debug.Log("OnAfterDeserialize");
        }
    }
}
