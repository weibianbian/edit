public interface IStateMachine
{
    void StateCanExit();

    void RequestStateChange(EStateType name, bool forceInstantly = false);

    StateBase ActiveState { get; }
    EStateType ActiveStateName { get; }
}
