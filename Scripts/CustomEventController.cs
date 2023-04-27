using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Diagnostics;

public class CustomEventController
{
    [ShowInInspector]
    [HideReferenceObjectPicker]
    [ListDrawerSettings(CustomAddFunction = "AddCustomEventAction")]
    public List<CustomEvent> customEvents = new List<CustomEvent>();
    public CustomEvent AddCustomEventAction => new CustomEvent();

    public CustomEvent nextEvent = null;
    public int nextIndex = GameConst.NotInitialized;

    private bool bRequestedFlowUpdate = false;

    public void StartEvent()
    {
        nextIndex = GameConst.NotInitialized;
        nextEvent = null;
        RequestExecution();
    }
    public void Update(float deltaTime)
    {
        if (bRequestedFlowUpdate)
        {
            bRequestedFlowUpdate = false;
            nextEvent = null;
            while (nextEvent==null)
            {
                int childIndex = FindEventToExecute();
                if (childIndex == (int)GameConst.ReturnToParent)
                {
                    break;
                }
                else if (IsValidIndex(childIndex))
                {
                    nextEvent = customEvents[childIndex];
                }
            }
            if (nextEvent==null)
            {
                //执行完毕，返回上一层 通知他的父级，
                UnityEngine.Debug.Log("执行完毕");
            }
            else
            {
                UnityEngine.Debug.Log("执行一个nextEvent");
                ExecuteEvent(nextEvent);
            }
        }
        if (nextEvent != null)
        {
            nextEvent.Update(this, deltaTime);
        }
    }
    public int FindEventToExecute()
    {
        int childIndex= GetNext(nextIndex);
        int RetIdx = (int)GameConst.ReturnToParent;
        if (IsValidIndex(childIndex))
        {
            nextIndex = childIndex;
            //这里可以选择初始化
            RetIdx = childIndex;
        }
        return RetIdx;
    }
    public int GetNext(int lastIndex)
    {
        int nextChildIndex = GameConst.ReturnToParent;
        if (lastIndex + 1 < customEvents.Count)
        {
            nextChildIndex = (lastIndex + 1);
        }
        return nextChildIndex;
    }
    public bool IsValidIndex(int index)
    {
        return index >= 0 && index < customEvents.Count;
    }
    public void RequestExecution()
    {
        bRequestedFlowUpdate = true;
    }
    public void OnChildFinished()
    {
        RequestExecution();
    }
    public void ExecuteEvent(CustomEvent evt)
    {
        evt.Start();
    }
}
