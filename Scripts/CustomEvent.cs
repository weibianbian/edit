using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;

public class CustomEvent
{
    [LabelText("事件名称")]
    public string eventName = "";
    [LabelText("事件集合")]
    [HideReferenceObjectPicker]
    public List<CustomEventData> datas = new List<CustomEventData>();

    private bool bRequestedFlowUpdate = false;
    private CustomEventData nextEventData;
    private int currentIndex = -1;
    private ActionBase curAction = null;
    public void Update()
    {
        if (bRequestedFlowUpdate)
        {
            bRequestedFlowUpdate = false;
            ProcessExecutionRequest();
        }
        if (curAction!=null)
        {
            //curAction
        }
    }
    public void ProcessExecutionRequest()
    {
        nextEventData = null;

        while (nextEventData == null)
        {
            int childIdx = FindEventToExecute();
            if (childIdx == (int)GameConst.ReturnToParent)
            {
            }
            else if (IsValidIndex(childIdx))
            {
                nextEventData = datas[childIdx];
            }
        }
        ProcessPendingExecution();
    }
    public void ProcessPendingExecution()
    {
        if (nextEventData != null)
        {
            ExecuteEventData(nextEventData);
        }
        else
        {
            OnCustomEventFinished();
        }
    }
    public int FindEventToExecute()
    {
        int childIdx = GetNextChild(currentIndex);
        int RetIdx = GameConst.ReturnToParent;
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
    public int GetNextChild(int lastIndex)
    {
        int nextChildIndex = GameConst.ReturnToParent;
        if (lastIndex + 1 < datas.Count)
        {
            nextChildIndex = (lastIndex + 1);
        }
        return nextChildIndex;
    }
    public void OnChildActivation(int index)
    {
        currentIndex = index;
    }
    public bool DoConditionAllowExecution(int childIndex)
    {
        CustomEventData evtData = datas[childIndex];
        bool result = true;
        if (evtData.conditions.Count == 0)
        {
            return result;
        }
        for (int conditionIndex = 0; conditionIndex < evtData.conditions.Count; conditionIndex++)
        {
            ConditionData TestCondition = evtData.conditions[conditionIndex];
            //bool bIsAllowed = (TestCondition != null) ? TestCondition.WrappedCanExecute() : false;

            //if (!bIsAllowed)
            //{
            //    result = false;
            //    break;
            //}
        }
        return result;
    }
    public bool IsValidIndex(int index)
    {
        return index >= 0 && index < datas.Count;
    }
    public void OnActivtion()
    {
        currentIndex = -1;
    }
    public void ExecuteEventData(CustomEventData data)
    {
        if (data.isBlock)
        {
            ActionBase action = ActionFactory.Create(data.actionType);
            EActionResult result = action.Execute(data);
            OnCustomEventDataFinished(data, result);
        }
        else
        {

        }
    }
    public void OnCustomEventDataFinished(CustomEventData data, EActionResult result)
    {
        if (result != EActionResult.InProgress)
        {
            //data.WrappedOnTaskFinished(result);

            RequestExecution();
        }
    }
    public void OnCustomEventFinished()
    {

    }
    public void RequestExecution()
    {
        bRequestedFlowUpdate = true;
    }
}
