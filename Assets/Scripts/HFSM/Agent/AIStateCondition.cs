public class AIStateCondition : ConditionBase
{
    public EAgentSubStateType stateType;
    public override bool Check(FSMComponent compt)
    {
        UnityEngine.Debug.LogError("AIStateCondition");
        return compt.agent.subState == stateType;
    }
}