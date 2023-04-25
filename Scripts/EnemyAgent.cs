using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.TerrainTools;
using UnityEditor;
using UnityEngine;
public class WaypointData
{
    [ShowInInspector]
    [HideReferenceObjectPicker]
    [ValueDropdown("@EditorTool.dataTypes", DrawDropdownForListElements = false)]
    public List<CompositeNode> datas;
    public bool bRequestedFlowUpdate = false;

    public CompositeNode testNode;
    public TaskNode nextTask;

    public Vector3 position;
    public WaypointData()
    {
        datas = new List<CompositeNode>();
        RequestExecution(ENodeResult.Succeeded);
    }

    private void Awake()
    {
        //Debug.Log(JsonConvert.SerializeObject(datas,Formatting.Indented));
    }
    private void Start()
    {
        testNode = datas[0];
        RequestExecution(ENodeResult.Succeeded);
    }
    public void Update()
    {
        if (bRequestedFlowUpdate)
        {
            bRequestedFlowUpdate = false;
            ProcessExecutionRequest();
            return;
        }
        if (testNode != null)
        {
            if (testNode.activeTask != null)
            {
                testNode.activeTask.WrappedTickTask(Time.deltaTime);
            }
        }
    }
    public void ProcessExecutionRequest()
    {
        nextTask = null;

        while (testNode != null && nextTask == null)
        {
            int childIdx = testNode.FindChildToExecute();
            if (childIdx == (int)ENodeResult.ReturnToParent)
            {
                //Composite已经执行完毕
            }
            else if (testNode.IsValidIndex(childIdx))
            {
                nextTask = testNode.tasks[childIdx];
            }
        }
        ProcessPendingExecution();
    }
    public void ProcessPendingExecution()
    {
        if (nextTask != null)
        {
            ExecuteTask(nextTask);
        }
        else
        {
            OnCompositeFinished();
        }
    }
    public void ExecuteTask(TaskNode taskNode)
    {
        testNode.activeTask = taskNode;

        ENodeResult result = taskNode.WrappedExecuteTask();

        OnTaskFinished(taskNode, result);
    }
    //-------------------
    public void OnTaskFinished(TaskNode taskNode, ENodeResult taskResult)
    {
        if (taskResult != ENodeResult.InProgress)
        {
            taskNode.WrappedOnTaskFinished(taskResult);

            RequestExecution(taskResult);
        }
    }
    public void RequestExecution(ENodeResult nodeResult)
    {
        bRequestedFlowUpdate = true;
    }
    //------------------------
    public void OnCompositeFinished()
    {

    }
}
