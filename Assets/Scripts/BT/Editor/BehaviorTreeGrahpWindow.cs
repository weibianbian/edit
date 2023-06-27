using BT.Runtime;
using GraphProcessor;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BT.Editor
{
    public class BehaviorTreeGrahpWindow: EditorWindow
    {
        UnityEditor.Experimental.GraphView.Blackboard xxxxxxx;
        [MenuItem("Window/Open BehaviorTree GraphWindow")]
        public static void Open()
        {
            BehaviorTreeGrahpWindow window = GetWindow<BehaviorTreeGrahpWindow>();
            window.minSize = new Vector2(700, 500);
        }
        protected VisualElement rootView;
        protected BehaviorTreeGraphView graphView;
        protected BehaviorTree treeAsset;

        readonly string graphWindowStyle= "GraphProcessorStyles/BaseGraphView";
        public void OnEnable()
        {
            rootVisualElement.Clear();
            InitializeRootView();

            LoadGraph();
            //if (graph != null)
            //    LoadGraph();
            //else
            //    reloadWorkaround = true;
        }
        void InitializeRootView()
        {
            rootView = base.rootVisualElement;

            rootView.name = "graphRootView";

            titleContent = new GUIContent("BehaviorTree Graph",
                AssetDatabase.LoadAssetAtPath<Texture2D>($"{GraphCreateAndSaveHelper.NodeGraphProcessorPathPrefix}/Editor/Icon_Dark.png"));
            rootView.styleSheets.Add(Resources.Load<StyleSheet>(graphWindowStyle));
        }
        void LoadGraph()
        {
            InitializeGraph(null);
        }
        public void InitializeGraph(BehaviorTree graph)
        {
            treeAsset=graph;
            InitializeWindow(graph);
            graphView.Initialize(graph);
        }
        protected void InitializeWindow(BehaviorTree graph)
        {
            graphView = new BehaviorTreeGraphView(this)
            {
               
            };
            
            rootView.Add(graphView);
            graphView.Add(new BTToolbarView(graphView));


            //Debug.LogError($"InitializeWindow  {(graph as BehaviorTreeGraph).behaviorTree}");
        }
        //protected override void OnEnable()
        //{
        //    base.OnEnable();

        //    titleContent = new GUIContent("BehaviorTree Graph",
        //        AssetDatabase.LoadAssetAtPath<Texture2D>($"{GraphCreateAndSaveHelper.NodeGraphProcessorPathPrefix}/Editor/Icon_Dark.png"));
        //    m_HasInitGUIStyles = false;
        //}

        //private bool m_HasInitGUIStyles;
        //protected override void InitializeWindow(BaseGraph graph)
        //{
        //    var graphView = new BehaviorTreeGraphView(this);
        //    rootView.Add(graphView);

        //    graphView.Add(new BTToolbarView(graphView));

        //    Debug.LogError($"InitializeWindow  {(graph as BehaviorTreeGraph).behaviorTree}");
        //}
        //private void OnGUI()
        //{
        //    InitGUIStyles(ref m_HasInitGUIStyles);

        //}
        //private void InitGUIStyles(ref bool result)
        //{
        //    if (!result)
        //    {
        //        EditorGUIStyleHelper.SetGUIStylePadding(nameof(EditorStyles.toolbarButton), new RectOffset(15, 15, 0, 0));
        //        result = true;
        //    }
        //}
    }

}

