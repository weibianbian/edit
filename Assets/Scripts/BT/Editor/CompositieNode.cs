using CopyBT;
using GraphProcessor;
using System.Threading.Tasks;
using UnityEngine;

namespace BT.Editor
{
    public class CompositieNode : BehaviourNode
    {
        [Input(name = "", allowMultiple = false), Vertical]
        public BehaviourNode input;
        [Output("", true), Vertical]
        public BehaviourNode output;
        public override Color color => new Color(0.1f, 0.3f, 0.7f);
        public override void Reset()
        {
            base.Reset();
            idx = 0;
        }
    }
}
