using BT.Runtime;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BT.Editor
{
    public class BTBBInspector : VisualElement
    {
        protected Label titleLable;
        protected VisualElement currentInspector;
        private Color c = new Color(93 / 255f, 93 / 255f, 93 / 255f);
        protected ScrollView scrollView;
        private int itemWidth = 110;
        private int space = 2;
        public BTBBInspector(EditorWindow aEditorWindow)
        {
            style.flexDirection = FlexDirection.Column;
            style.maxHeight = 400;
            titleLable = new Label();
            titleLable.style.color = c;
            titleLable.style.fontSize = 22;
            titleLable.style.unityFontStyleAndWeight = FontStyle.Bold;
            titleLable.style.alignContent = Align.Center;
            Add(titleLable);

            Add(VisualElementUtils.GetHorizontalLine(4, c));
            titleLable.text = "黑板面板";

            currentInspector = new VisualElement();
            Add(currentInspector);
            //Show(null);
        }

        public void Show(BTBlackboardData data)
        {
            currentInspector.Clear();

            scrollView = new ScrollView();
            scrollView.verticalScroller.style.width = 4;
            //FieldInfo[] fields = TypeUtils.GetAllFields(node.GetType());

            //foreach (var item in fields)
            //{
            //    CheckFieldInfo(item, node);
            //}
            currentInspector.Add(scrollView);
            if (data != null)
            {

            }
        }
        private void ShowString(BlackboardEntry entry, string value)
        {
            VisualElement line = VisualElementUtils.GetRowContainer();
            Label label = GetTitle(entry.entryName, 100);

            TextField stringField = new TextField();
            stringField.multiline = true;
            stringField.value = value;
            stringField.style.width = itemWidth;
            stringField.RegisterCallback<FocusInEvent>((e) => { Input.imeCompositionMode = IMECompositionMode.On; });
            stringField.RegisterCallback<FocusOutEvent>((e) => { Input.imeCompositionMode = IMECompositionMode.Auto; });
            stringField.RegisterCallback<ChangeEvent<string>>(evt =>
            {
                
            });
            line.Add(label);
            line.Add(stringField);
            scrollView.Add(line);
            scrollView.Add(VisualElementUtils.GetSpace(0, space));
        }
        public Label GetTitle(string title, int width)
        {
            Label label = new Label(title);
            label.style.unityTextAlign = TextAnchor.MiddleLeft;
            label.style.width = width;
            return label;
        }
    }
}
