using CopyBT;
using GraphProcessor;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

namespace BehaviorTree.Runtime
{
    [System.Serializable]
    public class BTCompositeNode: BTNode
    {
        [Input(name = "", allowMultiple=false), Vertical]
        public ENodeStatus input;
        [Output("", true), Vertical]
        public ENodeStatus output;
        public override bool isRenamable => false;

        public virtual int GetChild(int preChild, ENodeStatus lastResult)
        {
            return 0;
        }
        public int GetChildrenNum()
        {
            if (outputPorts.Count > 0)
            {
                return outputPorts[0].GetEdges().Count;
            }
            return 0;
        }
        public override void SaveStatus()
        {
            lastResult = status;
            //if (Child != null)
            //{
            //    for (int i = 0; i < Child.Count; i++)
            //    {
            //        Child[i].SaveStatus();
            //    }
            //}
        }
        public BTNode Child
        {
            get
            {
                if (outputPorts.Count > 0)
                {
                    var edges = outputPorts[0].GetEdges();
                    if (edges.Count > 0)
                    {
                        return edges[0].inputNode as BTNode;
                    }
                }
                return null;
            }
        }
    }
}

