using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

namespace CopyBT
{
    public class BehaviourNode
    {
        public static int NODE_COUNT = 0;
        public string name;
        public List<BehaviourNode> children;
        public BehaviourNode parent;
        public ENodeStatus status = ENodeStatus.READY;
        public ENodeStatus lastResult = ENodeStatus.READY;
        public float nextUpdateTick = 0;
        public int idx = 0;

        public BehaviourNode(string name):this(name,null)
        {

        }
        public BehaviourNode(string name, List<BehaviourNode> children)
        {
            this.name = name;
            this.children = children;
            status = ENodeStatus.READY;
            lastResult = ENodeStatus.READY;
            nextUpdateTick = 0;
            idx = BehaviourNode.NODE_COUNT;
            BehaviourNode.NODE_COUNT++;

            if (children != null)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].parent = this;
                }
            }
        }
        public virtual void Visit()
        {

        }
        public virtual void Step()
        {
            if (status != ENodeStatus.RUNNING)
            {
                Reset();
            }
            else if (children != null)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].Step();
                }
            }
        }
        public virtual void Reset()
        {

        }
    }
}

