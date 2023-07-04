using System;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace BT.Runtime
{
    [Serializable]
    public class BTCompositieNode : BTNode
    {
        public List<BTNode> childrens = new List<BTNode>();
        public override void Reset()
        {
            base.Reset();
            idx = 0;
        }
        protected int ChildCount
        {
            get
            {
                return childrens.Count;
            }
        }

        public BTNode ChildAtIndex(int index)
        {
            if (IsValidIndex(index))
            {
                return childrens[index];
            }
            return null;
        }
        public bool IsValidIndex(int idx)
        {
            return idx >= 0 && idx < ChildCount;
        }
    }
}
