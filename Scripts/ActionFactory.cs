public class ActionFactory
{
    public static ActionBase Create(EActionType actionType)
    {
        if (actionType == EActionType.CreateAI)
        {
            return new CreateAIAction();
        }
        return null;
    }
}
