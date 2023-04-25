public class CustomEventExecutor
{
    public CustomEventData data;

    public EActionResult eventResult= EActionResult.Successed;

    public float curTime = 0;
    public void OnNodeActivation()
    {
        curTime = 0;
    }
    public EActionResult Execute()
    {
        ActionBase action = ActionFactory.Create(data.actionType);
        if (action!=null)
        {
           return  action.Execute();
        }
        return EActionResult.Successed;
    }
    public void WrappedOnTaskFinished(EActionResult result)
    {

    }
    public void Update()
    {
        if (curTime>data.delayTime)
        {

        }
    }
}
