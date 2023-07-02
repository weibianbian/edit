using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BT.Editor
{
    public class Board : VisualElement
    {
        protected Label titleLable;

        private Color c = new Color(93 / 255f, 93 / 255f, 93 / 255f);
        public Board(EditorWindow aEditorWindow)
        {
            //mEditorWindow = aEditorWindow as GraphLogicWindow;

            style.flexDirection = FlexDirection.Column;

            titleLable = new Label();
            titleLable.style.color = c;
            titleLable.style.fontSize = 25;
            titleLable.style.unityFontStyleAndWeight = FontStyle.Bold;
            titleLable.style.alignContent = Align.Center;
            Add(titleLable);

            Add(GetHorizontalLine(4, c));

            InitStyle();
        }
        public VisualElement GetHorizontalLine(int lineWidth, Color r)
        {
            VisualElement line = new VisualElement()
            {
                style = { flexGrow = 0, height = 2, borderBottomWidth = lineWidth, borderBottomColor = r }
            };
            return line;
        }
        protected void InitStyle()
        {
            style.flexGrow = 1;
            style.backgroundColor = new Color(60 / 255f, 60 / 255f, 60 / 255f);
            style.marginLeft = 2;
            style.marginRight = 2;
            style.marginBottom = 4;
            style.marginTop = 2;

            style.paddingLeft = 2;
            style.paddingBottom = 2;
            style.paddingTop = 2;
            style.paddingRight = 2;

            style.borderLeftWidth = 1;
            style.borderBottomWidth = 1;
            style.borderRightWidth = 1;
            style.borderTopWidth = 1;
        }
    }
}
