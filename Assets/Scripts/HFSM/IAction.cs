namespace HFSM
{
    public interface IAction
    {
        void Execute(Game game, Entity entity);
    }
}

