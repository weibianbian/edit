using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BT.Editor
{
    public class InspectorBase : VisualElement
    {
        protected Label titleLable;

        private Color c = new Color(93 / 255f, 93 / 255f, 93 / 255f);
        public InspectorBase(EditorWindow aEditorWindow)
        {
            //mEditorWindow = aEditorWindow as GraphLogicWindow;

            style.flexDirection = FlexDirection.Column;
            style.maxHeight = 600;

            titleLable = new Label();
            titleLable.style.color = c;
            titleLable.style.fontSize = 22;
            titleLable.style.unityFontStyleAndWeight = FontStyle.Bold;
            titleLable.style.alignContent = Align.Center;
            Add(titleLable);

            Add(VisualElementUtils.GetHorizontalLine(4, c));

            InitStyle();
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
