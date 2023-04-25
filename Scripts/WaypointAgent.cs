using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class GameConst
{
    public const int ReturnToParent = -2;
}

public class WaypointAgent:SerializedMonoBehaviour
{
    private Color color = new Color(1, 0, 0);
    public float radius = 0.25f;
    [LabelText("主事件")]
    [ShowInInspector]
    [HideReferenceObjectPicker]
    [ListDrawerSettings(CustomAddFunction = "AddCustomEventAction")]
    public List<CustomEvent> events = new List<CustomEvent>();
    public CustomEvent AddCustomEventAction => new CustomEvent();

    public CustomEvent nextEvent = null;
    public int currentIndex = -1;
    public bool bRequestedFlowUpdate = false;
    public const int ReturnToParent = -2;
    
    public void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);
        //if (waypoint != null)
        //{
        //    waypoint.position = transform.position;
        //}
    }
    public void StartExecuteList()
    {
        currentIndex = -1;
        nextEvent = null;
        RequestExecution();
    }
    public void Update()
    {
        if (bRequestedFlowUpdate)
        {
            bRequestedFlowUpdate = false;
            ProcessExecutionRequest();
        }
    }
    public void RequestExecution()
    {
        bRequestedFlowUpdate = true;
    }
    public void ProcessExecutionRequest()
    {
        nextEvent = null;

        while (nextEvent == null)
        {
            int childIdx = FindChildToExecute();
            if (childIdx == (int)ENodeResult.ReturnToParent)
            {
            }
            else if (IsValidIndex(childIdx))
            {
                nextEvent = events[childIdx];
            }
        }
        ProcessPendingExecution();
    }
    public int FindChildToExecute()
    {
        int childIdx = GetNextChild(currentIndex);
        int RetIdx = ReturnToParent;
        while (IsValidIndex(childIdx))
        {
            if (DoConditionAllowExecution(childIdx))
            {
                OnChildActivation(childIdx);
                RetIdx = childIdx;
                break;
            }
            childIdx = GetNextChild(childIdx);
        }
        return RetIdx;
    }
    public void OnChildActivation(int index)
    {
        currentIndex = index;
    }
    public int GetNextChild(int lastIndex)
    {
        int nextChildIndex = ReturnToParent;
        if (lastIndex + 1 < events.Count)
        {
            nextChildIndex = (lastIndex + 1);
        }
        return nextChildIndex;
    }
    public bool DoConditionAllowExecution(int childIndex)
    {
        CustomEvent evt = events[childIndex];
        bool result = true;
        //if (evt.conditions.Count == 0)
        //{
        //    return result;
        //}
        //for (int conditionIndex = 0; conditionIndex < events.data.conditions.Count; conditionIndex++)
        //{
        //    ConditionData TestCondition = executor.data.conditions[conditionIndex];
        //    //bool bIsAllowed = (TestCondition != null) ? TestCondition.WrappedCanExecute() : false;

        //    //if (!bIsAllowed)
        //    //{
        //    //    result = false;
        //    //    break;
        //    //}
        //}
        return result;
    }
    public bool IsValidIndex(int index)
    {
        return index >= 0 && index < events.Count;
    }
    public void ProcessPendingExecution()
    {
        if (nextEvent != null)
        {
            ExecuteEvent(nextEvent);
        }
        else
        {
            OnListFinished();
        }
    }
    public void ExecuteEvent(CustomEvent evt)
    {
        //EActionResult result = evt.Execute();

        //OnExecutorFinished(evt, result);
    }
    //-------------------
    public void OnExecutorFinished(CustomEventExecutor executor, EActionResult executorResult)
    {
        if (executorResult != EActionResult.InProgress)
        {
            executor.WrappedOnTaskFinished(executorResult);

            RequestExecution();
        }
    }
    public void OnListFinished()
    {

    }
}