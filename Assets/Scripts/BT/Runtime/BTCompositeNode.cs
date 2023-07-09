using System.Collections.Generic;

namespace BT.Runtime
{
    public class BTCompositeChild
    {
        public BTCompositeNode childComposite;
        public BTTaskNode childAction;
    }
    public class BTCompositeNode : BTNode
    {
        public List<BTCompositeChild> childrens = new List<BTCompositeChild>();
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

        public BTCompositeChild ChildAtIndex(int index)
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
