using UnityEngine;

public class CreateAIAction : EventActionBase
{
    protected override ENodeResult OnExecute(EventActionData data)
    {
        GameObject go = new GameObject("CreateAIAction");
        return ENodeResult.Succeeded;
    }
}
