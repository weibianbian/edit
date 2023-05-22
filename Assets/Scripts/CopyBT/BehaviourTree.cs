using System.Diagnostics;

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
            root.SaveStatus();
            root.Step();
            UnityEngine.Debug.Log(__ToString());
        }
        public void Reset()
        {
            root.Reset();
        }
        public string __ToString()
        {
            return root.GetTreeString();
        }
    }
}

