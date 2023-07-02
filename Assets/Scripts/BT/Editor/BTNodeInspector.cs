using BT.Runtime;
using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BT.Editor
{
    public class BTNodeInspector : Board
    {
        private ScrollView mScrollView;
        private int itemWidth = 110;
        private int space = 2;
        public BTNodeInspector(EditorWindow aEditorWindow) : base(aEditorWindow)
        {
            titleLable.text = "属性面板";
        }

        public void Show(BTNode node)
        {
            //Clear();
            mScrollView = new ScrollView();
            mScrollView.verticalScroller.style.width = 4;
            FieldInfo[] fields = TypeUtils.GetAllFields(node.GetType());

            foreach (var item in fields)
            {
                CheckFieldInfo(item, node);
            }
            Add(mScrollView);
        }
        private void CheckFieldInfo(FieldInfo info,object obj)
        {
            if (info.FieldType == typeof(bool))
            {
                ShowBool(info,obj);
                return;
            }
            if (info.FieldType == typeof(string))
            {
                ShowString(info, obj);
                return;
            }
            if (info.FieldType == typeof(int))
            {
                ShowInt(info, obj);
                return;
            }
            if (info.FieldType.IsEnum)
            {
                ShowEnum(info, obj);
                return;
            }
        }
        private void ShowEnum(FieldInfo info, object obj)
        {
            VisualElement line = VisualElementUtils.GetRowContainer();

            Label label = GetTitle((info.Name), 130);

            EnumField enumField = new EnumField((Enum)info.GetValue(obj));
            enumField.style.width = itemWidth;
            enumField.RegisterCallback<ChangeEvent<Enum>>(evt =>
            {
                info.SetValue(obj, evt.newValue);
            });
            line.Add(label);
            line.Add(enumField);
            mScrollView.Add(line);
            mScrollView.Add(VisualElementUtils.GetSpace(0, space));
        }
        private void ShowInt(FieldInfo info, object obj)
        {
            VisualElement line = VisualElementUtils.GetRowContainer();
            Label label = GetTitle((info.Name), 130);

            IntegerField intField = new IntegerField();
            intField.style.width = itemWidth;
            intField.value = (int)info.GetValue(obj);
            intField.RegisterCallback<ChangeEvent<int>>(evt =>
            {
                info.SetValue(obj, evt.newValue);
            });
            line.Add(label);
            line.Add(intField);
            mScrollView.Add(line);
            mScrollView.Add(VisualElementUtils.GetSpace(0, space));
        }
        private void ShowString(FieldInfo info, object obj)
        {
            VisualElement line = VisualElementUtils.GetRowContainer();
            Label label = GetTitle((info.Name), 100);

            TextField stringField = new TextField();
            stringField.multiline = true;
            stringField.value = (string)info.GetValue(obj);
            stringField.style.width = itemWidth;
            stringField.RegisterCallback<FocusInEvent>((e) => { Input.imeCompositionMode = IMECompositionMode.On; });
            stringField.RegisterCallback<FocusOutEvent>((e) => { Input.imeCompositionMode = IMECompositionMode.Auto; });
            stringField.RegisterCallback<ChangeEvent<string>>(evt =>
            {
                info.SetValue(obj, evt.newValue);
            });
            line.Add(label);
            line.Add(stringField);
            mScrollView.Add(line);
            mScrollView.Add(GetSpace(0, space));
        }
        private void ShowBool(FieldInfo info,object obj)
        {
            VisualElement line = VisualElementUtils.GetRowContainer();
            Label label = GetTitle((info.Name), 224);

            Toggle boolField = new Toggle();
            boolField.value = (bool)info.GetValue(obj);
            boolField.RegisterCallback<ChangeEvent<bool>>(evt =>
            {
                info.SetValue(obj, evt.newValue);
            });
            line.Add(label);
            line.Add(boolField);
            mScrollView.Add(line);
            mScrollView.Add(GetSpace(0, space));
        }
        public VisualElement GetSpace(int _width, int _height)
        {
            VisualElement space = new VisualElement()
            {
                style = { width = _width, height = _height }
            };
            return space;
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
