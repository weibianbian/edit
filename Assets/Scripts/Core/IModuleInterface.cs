using RailShootGame;

namespace Core
{
    public interface IModuleInterface
    {
        void StartupModule();
        void ShutdownModule();
    }
    public interface IGameplayTaskOwnerInterface
    {
        bool IsValid();
        void OnGameplayTaskActivated(UGameplayTask Task);
        void OnGameplayTaskDeactivated(UGameplayTask Task);
        void OnGameplayTaskInitialized(UGameplayTask Task);
        UGameplayTasksComponent GetGameplayTasksComponent(UGameplayTask Task);
    }
}

