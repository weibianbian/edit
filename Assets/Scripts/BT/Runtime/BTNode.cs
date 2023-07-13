using System;
using UnityEditor.VersionControl;
using UnityEngine;

namespace BT.Runtime
{
    [Serializable]
    public class BTNode : ISerializationCallbackReceiver
    {
        public ENodeStatus status = ENodeStatus.READY;
        public ENodeStatus lastResult = ENodeStatus.READY;
        public float nextUpdateTick = 0;
        public int idx = 0;
        public Action onVisit;
        [EditAnywhere]
        public string nodeName = "";

        public int executionIndex = 0;
        public int treeDepth = 0;
        public BehaviorTree treeAsset;
        public void InitializeNode(BTCompositeNode inParentNode, int inExecutionIndex, int inTreeDepth)
        {
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

        public BTBlackboardData GetBlackboardAsset()
        {
            return treeAsset.blackboardData;
        }
        public virtual void InitializeFromAsset(BehaviorTree asset)
        {
            treeAsset = asset;
        }
    }
}
