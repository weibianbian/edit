using RailShootGame;

namespace HFSMRuntime
{
    public interface IAction
    {
        void Execute(UWorld game, AActor entity);
    }
}

