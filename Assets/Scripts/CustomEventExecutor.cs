public class CustomEventExecutor
{
    public EventActionData data;

    public ENodeResult eventResult = ENodeResult.Succeeded;

    public float curTime = 0;
    public void OnNodeActivation()
    {
        curTime = 0;
    }
    public ENodeResult Execute()
    {
        EventActionBase action = ActionFactory.Create(data.actionType);
        if (action!=null)
        {
           return  action.Execute(data);
        }
        return ENodeResult.Succeeded;
    }
    public void WrappedOnTaskFinished(ENodeResult result)
    {

    }
    public void Update()
    {
        if (curTime>data.delayTime)
        {

        }
    }
}
