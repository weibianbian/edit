public interface IMeStateMachine
{
    void StateCanExit();

    void RequestStateChange(string name, bool forceInstantly = false);

    MeStateBase ActiveState { get; }
    string ActiveStateName { get; }
}
