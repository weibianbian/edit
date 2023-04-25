public abstract class ActionBase
{
    public  EActionResult Execute(CustomEventData data)
    {
        return OnExecute();
    }
    protected virtual EActionResult OnExecute()
    {
        return EActionResult.Successed;
    }
}
