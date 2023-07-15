using RailShootGame;

namespace HFSMRuntime
{
    public interface IAction
    {
        void Execute(Game game, Actor entity);
    }
}

