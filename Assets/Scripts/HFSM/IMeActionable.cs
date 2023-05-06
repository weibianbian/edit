public interface IMeActionable
{
    void OnAction(string trigger);
    void OnAction<TData>(string trigger, TData data);
}
