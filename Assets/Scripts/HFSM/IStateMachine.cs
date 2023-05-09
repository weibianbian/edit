public interface IStateMachine
{
    bool StateCanExit();

    void RequestStateChange(string name, bool forceInstantly = false);

    StateBase ActiveState { get; }
    string ActiveStateName { get; }
}
