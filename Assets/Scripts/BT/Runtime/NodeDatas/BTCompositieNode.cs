using System.Collections.Generic;

namespace BT.Runtime
{
    public class BTCompositieNode : BTNode
    {
        protected List<BTNode> childrens = new List<BTNode>();
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

        protected BTNode ChildAtIndex(int index)
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
