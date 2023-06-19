using System;
using System.Collections.Generic;
using System.Text;

namespace CopyBT
{
    public class CopyBTBehaviourNode
    {
        public static int NODE_COUNT = 0;
        public string name;
        public List<CopyBTBehaviourNode> children;
        public CopyBTBehaviourNode parent;
        public ENodeStatus status = ENodeStatus.READY;
        public ENodeStatus lastResult = ENodeStatus.READY;
        public float nextUpdateTick = 0;
        public int idx = 0;

        public CopyBTBehaviourNode(string name) : this(name, null)
        {

        }
        public CopyBTBehaviourNode(string name, List<CopyBTBehaviourNode> children)
        {
            this.name = name;
            this.children = children;
            status = ENodeStatus.READY;
            lastResult = ENodeStatus.READY;
            nextUpdateTick = 0;
            idx = CopyBTBehaviourNode.NODE_COUNT;
            CopyBTBehaviourNode.NODE_COUNT++;

            if (children != null)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].parent = this;
                }
            }
        }
        public virtual string DBString()
        {
            return "";
        }
        public virtual void Visit()
        {

        }
        public  virtual void DoToParents(Action<CopyBTBehaviourNode> fn)
        {
            if (parent!=null)
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
            if (status != ENodeStatus.READY)
            {
                status = ENodeStatus.READY;
                if (children != null)
                {
                    for (int i = 0; i < children.Count; i++)
                    {
                        children[i].Reset();
                    }
                }
            }
        }
        public void SaveStatus()
        {
            lastResult = status;
            if (children != null)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].SaveStatus();
                }
            }
        }
        public bool IsValidIndex(int idx)
        {
            return idx >= 0 && idx < children.Count;
        }
        public string GetTreeString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"{GetString()}");
            if (children != null)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    stringBuilder.Append($"{children[i].GetTreeString()}   >");
                }
            }
            return stringBuilder.ToString();
        }
        public string GetString()
        {
            string str = "";
            if (status == ENodeStatus.RUNNING)
            {
                str = DBString();
            }
            return $"{name}-{status} <{lastResult}>  ({str})";
        }
    }
}

