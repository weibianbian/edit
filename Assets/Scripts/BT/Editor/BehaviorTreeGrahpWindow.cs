using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BT.Editor
{
    public class BehaviorTreeGrahpWindow : EditorWindow
    {
        UnityEditor.Experimental.GraphView.Blackboard xxxxxxx;
        public BTNodeInspector nodeInspector;
        [MenuItem("Window/Open BehaviorTree GraphWindow")]
        public static void Open()
        {
            BehaviorTreeGrahpWindow window = GetWindow<BehaviorTreeGrahpWindow>();
            window.minSize = new Vector2(700, 500);
        }
        protected VisualElement rootView;
        protected BehaviorTreeGraphView graphView;

        protected VisualElement leftContainer;
        protected VisualElement mainContainer;
        protected Color boardColor = new Color(240 / 255f, 250 / 255f, 180 / 255f);
        readonly string graphWindowStyle = "GraphProcessorStyles/BaseGraphView";
        public void OnEnable()
        {
            rootVisualElement.Clear();
            InitializeRootView();
            InitElementView();


            LoadGraph();

            nodeInspector = new BTNodeInspector(this);

            leftContainer.Add(nodeInspector);
            nodeInspector.Show(new Runtime.BTNode());
        }
        public void InitElementView()
        {
            mainContainer = new VisualElement()
            {
                style = { flexGrow = 1, flexBasis = 0, flexDirection = FlexDirection.Row },
            };
            mainContainer.pickingMode = PickingMode.Ignore;
            rootVisualElement.Add(mainContainer);

            rootVisualElement.Insert(0, new BTToolbarView());

            leftContainer = new VisualElement()
            {
                style = { flexGrow = 0, flexDirection = FlexDirection.Column, width = 260 }
            };
            mainContainer.Add(leftContainer);
        }
        public void InitToolbarView()
        {

        }
        void InitializeRootView()
        {
            rootView = base.rootVisualElement;

            rootView.name = "graphRootView";

            titleContent = new GUIContent("BehaviorTree Graph");
            //    AssetDatabase.LoadAssetAtPath<Texture2D>($"{GraphCreateAndSaveHelper.NodeGraphProcessorPathPrefix}/Editor/Icon_Dark.png"));
            //rootView.styleSheets.Add(Resources.Load<StyleSheet>(graphWindowStyle));
        }
        void LoadGraph()
        {
            InitializeGraph();
        }
        public void InitializeGraph()
        {
            InitializeWindow();
            graphView.Initialize();
        }
        protected void InitializeWindow()
        {
            graphView = new BehaviorTreeGraphView(this)
            {
                style = { flexGrow = 1,flexBasis =0, marginLeft = 0, marginRight = 2, marginBottom = 2,
                marginTop = 0, paddingLeft = 0, paddingBottom = 1, paddingTop = 1, paddingRight = 0,
                borderLeftColor = boardColor, borderRightColor = boardColor, borderBottomColor = boardColor, borderTopColor = boardColor,
                borderLeftWidth = 1,borderBottomWidth = 1,borderRightWidth = 1,borderTopWidth = 1,
            },
            };

            mainContainer.Insert(0, graphView);



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

