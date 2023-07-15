using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

// help us to create waypoints
public class WaypointPanel : EditorWindow
{
    static WaypointPanel sWinInst = null;

    public float createCount = 3;
    public string holderName = "";
    private bool bValidName = false;
    private string rootName = "WaypointRoot";

    public Vector3 startPosition = new Vector3(0, 0, 0);

    [@MenuItem("Tools/Waypoint Path Creator(·���༭)")]
    public static void ShowWindow()
    {
        GetInstance().Show();
    }

    public static WaypointPanel GetInstance()
    {
        if (sWinInst == null)
            sWinInst = (WaypointPanel)EditorWindow.GetWindow(typeof(WaypointPanel));
        return sWinInst;
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("��ʼ������", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical();
        {
            holderName = EditorGUILayout.TextField("����:", holderName);
            createCount = EditorGUILayout.Slider("·�����:", createCount, 1, 200);
            startPosition = EditorGUILayout.Vector3Field("��ʼλ��", startPosition);

            bValidName = IsValidName();

            EditorGUILayout.BeginToggleGroup("��Ч����", bValidName);
            if (GUILayout.Button("����"))
            {
                CreateWapoints();
            }
            EditorGUILayout.EndToggleGroup();
        }
        EditorGUILayout.EndVertical();
    }

    private bool IsValidName()
    {
        return holderName != "" && !GameObject.Find($"{rootName}/{holderName}");
    }

    private void CreateWapoints()
    {
        GameObject RootObj = new GameObject(holderName);
        RootObj.transform.parent = GameObject.Find($"{rootName}").transform;
        WaypointGroup wpGroup = RootObj.AddComponent<WaypointGroup>();

        Transform RootTransform = RootObj.transform;
        RootTransform.position = startPosition;

        for (int i = 0; i < createCount; ++i)
        {
            GameObject go = new GameObject("Waypoint" + i);

            var trans = go.transform;
            trans.parent = RootTransform;
            WaypointObject obj = go.AddComponent<WaypointObject>();

            trans.position = startPosition + new Vector3(i * 4, 0, 0);
            wpGroup.waypoints.Add(obj);
        }
    }
}

