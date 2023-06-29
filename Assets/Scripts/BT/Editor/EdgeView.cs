using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BT.Editor
{
    public class EdgeView : Edge
    {
        readonly string edgeStyle = "GraphStyles/EdgeView";
        public bool isConnected = false;
        protected BehaviorTreeGraphView owner => ((input ?? output) as NodePortView).owner.owner;
        public EdgeView() : base()
        {
            styleSheets.Add(Resources.Load<StyleSheet>(edgeStyle));
            RegisterCallback<MouseDownEvent>(OnMouseDown);
        }
        public void UpdateEdgeSize()
        {
            if (input == null && output == null)
                return;

            //PortData inputPortData = (input as PortView)?.portData;
            //PortData outputPortData = (output as PortView)?.portData;

            //for (int i = 1; i < 20; i++)
            //    RemoveFromClassList($"edge_{i}");
            //int maxPortSize = Mathf.Max(inputPortData?.sizeInPixel ?? 0, outputPortData?.sizeInPixel ?? 0);
            //if (maxPortSize > 0)
            //    AddToClassList($"edge_{Mathf.Max(1, maxPortSize - 6)}");
        }
        void OnMouseDown(MouseDownEvent e)
        {
            if (e.clickCount == 2)
            {
                // Empirical offset:
                var position = e.mousePosition;
                position += new Vector2(-10f, -28);
                Vector2 mousePos = owner.ChangeCoordinatesTo(owner.contentViewContainer, position);
            }
        }
    }
}

