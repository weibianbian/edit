using Sirenix.OdinInspector;
using System.Collections.Generic;

public class DelayEventWraper
{
    public float delayTime = 0;
    public bool completed = false;
    public EventActionData evt;
    public void Update(float deltaTime)
    {
        if (completed) return;
        if (delayTime <= 0)
        {
            completed = true;
        }
        else
        {
            delayTime -= deltaTime;
        }
    }
    public bool IsCompleted()
    {
        return completed;
    }
}
public class CustomEvent
{
    [LabelText("事件名称")]
    public string eventName = "";
    [LabelText("事件集合")]
    [HideReferenceObjectPicker]
    [ListDrawerSettings(CustomAddFunction = "AddEventActionData")]
    public List<EventActionData> datas = new List<EventActionData>();
    public EventActionData AddEventActionData => new EventActionData();
    private bool bRequestedFlowUpdate = false;
    private EventActionData nextEventData;
    private int currentIndex = GameConst.NotInitialized;

    private DelayEventWraper delayEventWraper = null;
    public void Start()
    {
        currentIndex = GameConst.NotInitialized;
        RequestExecution();
    }
    public void Update(CustomEventController parent, float deltaTime)
    {
        if (bRequestedFlowUpdate)
        {
            bRequestedFlowUpdate = false;
            ProcessExecutionRequest(parent);
        }
        if (delayEventWraper != null)
        {
            delayEventWraper.Update(deltaTime);
            if (delayEventWraper.IsCompleted())
            {
                nextEventData = delayEventWraper.evt;
                delayEventWraper = null;
                ExecuteEventData(nextEventData);
            }
        }
        if (nextEventData != null)
        {
            nextEventData.Update(this, deltaTime);
        }
    }
    public void ProcessExecutionRequest(CustomEventController parent)
    {
        nextEventData = null;

        while (nextEventData == null)
        {
            int childIdx = FindEventToExecute();
            if (childIdx == (int)GameConst.ReturnToParent)
            {
                //执行完毕，返回上一层 通知他的父级，
                break;
            }
            else if (IsValidIndex(childIdx))
            {
                nextEventData = datas[childIdx];
            }
        }
        ProcessPendingExecution(parent);
    }
    public void ProcessPendingExecution(CustomEventController parent)
    {
        if (nextEventData != null)
        {
            if (nextEventData.delayTime > 0)
            {
                delayEventWraper = new DelayEventWraper();
                delayEventWraper.evt = nextEventData;
                delayEventWraper.delayTime = nextEventData.delayTime;
                nextEventData = null;
            }
            else
            {
                ExecuteEventData(nextEventData);
            }
        }
        else
        {
            OnCustomEventFinished(parent);
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
        datas[index].OnActivtion();
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
        if (!evtData.isWaitForConditionToHold)
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
        ENodeResult result = data.Execute(this);
        OnActionFinished(data, result);
    }
    public void OnActionFinished(EventActionData data, ENodeResult result)
    {
        if (result != ENodeResult.InProgress)
        {
            //data.WrappedOnActionFinished(result);
            nextEventData = null;
            RequestExecution();
        }
    }
    public void OnCustomEventFinished(CustomEventController parent)
    {
        parent.OnChildFinished();
    }
    public void RequestExecution()
    {
        bRequestedFlowUpdate = true;
    }
}
