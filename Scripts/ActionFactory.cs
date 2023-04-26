public class ActionFactory
{
    public static EventActionBase Create(EEventAction actionType)
    {
        if (actionType == EEventAction.CreateAI)
        {
            return new CreateAIAction();
        }
        return null;
    }
}
