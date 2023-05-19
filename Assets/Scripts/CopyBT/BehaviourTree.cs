namespace CopyBT
{

    public class BehaviourTree
    {
        BehaviourNode root = null;
        public BehaviourTree(BehaviourNode root)
        {
            this.root = root;
        }
        public void Update()
        {
            root.Visit();
            root.Step();
        }
        public void Reset()
        {
            root.Reset();
        }
    }
}

