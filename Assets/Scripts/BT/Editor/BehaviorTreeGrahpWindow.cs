using BT.Runtime;
using Newtonsoft.Json;
using System.IO;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager.UI;
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
            UnityEngine.Debug.Log($"BehaviorTreeGrahpWindow.OnEnable");
            rootVisualElement.Clear();
            InitializeRootView();
            InitElementView();


            LoadBTGraphView();

            nodeInspector = new BTNodeInspector(this);

            leftContainer.Add(nodeInspector);


        }
        public void InitElementView()
        {
            mainContainer = new VisualElement()
            {
                style = { flexGrow = 1, flexBasis = 0, flexDirection = FlexDirection.Row },
            };
            mainContainer.pickingMode = PickingMode.Ignore;
            rootVisualElement.Add(mainContainer);

            rootVisualElement.Insert(0, new BTToolbarView(this));

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
        void LoadBTGraphView()
        {
            byte[] bytes = LoadBehaviorTree();
            InitializeGraph();
            RestoreBehaviorTree(bytes);
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
        }

        public void OnSelectedNode(BehaviorGraphNodeView nodeView)
        {
            if (nodeView != null && nodeView.nodeInstance != null)
            {
                nodeInspector.Show(nodeView.nodeInstance);
            }
        }
        public void OnUnselectedNode(BehaviorGraphNodeView nodeView)
        {
            if (nodeInspector != null) { nodeInspector.ClearBoard(); }
        }
        public byte[] LoadBehaviorTree()
        {
            //打开资源
            string rootPath = $"{Application.dataPath}/treeAssets";
            string path = $"{rootPath}/001.Json";
            byte[] data = LoadFile(path);
            return data;

        }
        public byte[] LoadFile(string path)
        {
            if (File.Exists(path))
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    if (fs.Length != 0)
                    {
                        byte[] data = new byte[fs.Length];
                        fs.Read(data, 0, data.Length);
                        return data;
                    }
                }
            }
            return null;
        }
        public void RestoreBehaviorTree(byte[] data)
        {
            if (data == null)
            {
                graphView.treeAsset = new BehaviorTree();
            }
            else
            {
                string json = System.Text.Encoding.UTF8.GetString(data);
                graphView.treeAsset = JsonConvert.DeserializeObject<BehaviorTree>(json);
            }
            CreateDefaultNodesForGraph();
            graphView.OnCreated();
        }
        public void CreateDefaultNodesForGraph()
        {
            BTGraphNodeCreator<BehaviorGraphNodeRootView> nodeCreator = new BTGraphNodeCreator<BehaviorGraphNodeRootView>(graphView);
            BehaviorGraphNodeRootView myNode = nodeCreator.CreateNode();
            nodeCreator.OnFinalize();
        }
        public void CreateBehaviorTree()
        {
            Debug.Log("CreateBehaviorTree");
            graphView.treeAsset = new BehaviorTree();
        }
        public void SaveBehaviorTree()
        {
            if (graphView.treeAsset != null)
            {
                Debug.Log("SaveBehaviorTree");
                //var setting = new JsonSerializerSettings();
                //setting.Formatting = Formatting.Indented;
                //setting.TypeNameHandling = TypeNameHandling.All;
                //setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //string str = JsonConvert.SerializeObject(behaviorTree, setting);
                graphView.OnSave();
                //Debug.Log(str);
            }
        }
    }

}

