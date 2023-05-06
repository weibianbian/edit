public interface IActionable
{
    void OnAction(string trigger);
    void OnAction<TData>(string trigger, TData data);
}
