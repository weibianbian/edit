public interface IMeActionable<TEvent>
{
    void OnAction(TEvent trigger);
    void OnAction<TData>(TEvent trigger, TData data);
}
