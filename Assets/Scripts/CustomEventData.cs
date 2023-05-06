using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Diagnostics;

public class EventActionData
{
    [LabelText("条件")]
    public List<ConditionData> conditions = new List<ConditionData>();
    [LabelText("延迟时间（秒）")]
    public float delayTime = 0;
    [LabelText("动作ID")]
    public EEventAction actionType= EEventAction.None;
    [LabelText("原地创建")]
    public bool isCreateInPlace = false;
    [LabelText("阻塞")]
    public bool isBlock = true;
    [LabelText("等待条件成立")]
    public bool isWaitForConditionToHold = false;

    private EventActionBase eventAction = null;
    public void OnActivtion()
    {
        eventAction= ActionFactory.Create(actionType);
    }
    public void OnDeactivtion()
    {
        eventAction = null;
    }
    public ENodeResult Execute(CustomEvent parent)
    {
        ENodeResult result = ENodeResult.Succeeded;
        if (eventAction != null)
        {
            result = eventAction.Execute(this);
        }
        return isBlock?result: ENodeResult.Succeeded;
    }
    public void Update(CustomEvent parent,float deltaTime)
    {
        if (eventAction!=null)
        {
            eventAction.Update(this,deltaTime);
        }
        //parent.OnActionFinished(this, ENodeResult.Succeeded);
    }
}
public class ConditionData
{

}