using UEngine;
using UEngine.GameFramework;

namespace HFSMRuntime
{
    public interface IAction
    {
        void Execute(UWorld game, AActor entity);
    }
}

