using BT.Runtime;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static Codice.CM.WorkspaceServer.WorkspaceTreeDataStore;

namespace BT.Editor
{
    public class BTToolbarView : Toolbar
    {
        protected enum ElementType
        {
            Button,
            Toggle,
            DropDownButton,
            Separator,
            Custom,
            FlexibleSpace,
        }
        BehaviorTreeGrahpWindow window;
        public BTToolbarView(BehaviorTreeGrahpWindow window)
        {
            this.window = window;
            AddButtons();
        }
        protected void AddButtons()
        {
            AddButton("Center", () => { }, TextAnchor.MiddleLeft);
            AddButton("CreateBehaviorTree", CreateBehaviorTree, TextAnchor.MiddleLeft);
            AddButton("SaveBehaviorTree", SaveBehaviorTree, TextAnchor.MiddleLeft);
        }
        protected void AddButton(string content, Action callback, TextAnchor anchor)
        {
            var btn = new ToolbarButton(callback);
            btn.style.unityTextAlign = anchor;
            btn.text = content;
            Add(btn);
        }
        public void CreateBehaviorTree()
        {
            window.CreateBehaviorTree();
        }
        public void SaveBehaviorTree()
        {
            window.SaveBehaviorTree();
        }
    }
}

