using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;

public class CustomEvent
{
    [LabelText("事件名称")]
    public string eventName = "";
    [LabelText("事件集合")]
    [HideReferenceObjectPicker]
    public List<EventActionData> datas = new List<EventActionData>();

    private bool bRequestedFlowUpdate = false;
    private EventActionData nextEventData;
    private int currentIndex = -1;
    private EventActionBase nextEventAction;
    public void Update(float deltaTime)
    {
        if (bRequestedFlowUpdate)
        {
            bRequestedFlowUpdate = false;
            ProcessExecutionRequest();
        }
        if (nextEventAction!=null)
        {
            nextEventAction.Update(this, deltaTime);
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
                //执行完毕，返回上一层 通知他的父级，
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
        EventActionData evtData = datas[childIndex];
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
    public void ExecuteEventData(EventActionData data)
    {
        EventActionBase action = ActionFactory.Create(data.actionType);
        ENodeResult result = action.Execute(data);
        OnActionFinished(action, result);
    }
    public void OnActionFinished(EventActionBase action, ENodeResult result)
    {
        if (result != ENodeResult.InProgress)
        {
            action.WrappedOnActionFinished(result);

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
