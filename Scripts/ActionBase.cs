public abstract class EventActionBase
{
    public ENodeResult Execute(EventActionData data)
    {
        return OnExecute(data);
    }
    protected virtual ENodeResult OnExecute(EventActionData data)
    {
        ENodeResult result = ENodeResult.InProgress;
        if (!data.isBlock)
        {
            result= ENodeResult.Succeeded;
        }
        else
        {

        }
        return result;
    }
    public virtual void Update(CustomEvent parent, float deltaTime)
    {
        
    }
    public void FinishLatentTask(CustomEvent parent,ENodeResult result)
    {
        parent.OnActionFinished(this, result);
    }
    public void WrappedOnActionFinished(ENodeResult result)
    {

    }
}
