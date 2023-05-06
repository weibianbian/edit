public class TransitionBase
{
    public string from;
    public string to;

    public bool forceInstantly;

    public IStateMachine fsm;

    public TransitionBase(string from, string to, bool forceInstantly = false)
    {
        this.from = from;
        this.to = to;
        this.forceInstantly = forceInstantly;
    }
    public virtual void Init()
    {

    }
    public virtual void OnEnter()
    {

    }
    public virtual bool ShouldTransition()
    {
        return true;
    }
}
