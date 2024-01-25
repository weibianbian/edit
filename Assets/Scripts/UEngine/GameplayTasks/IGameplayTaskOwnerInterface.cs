namespace UEngine.GameplayTasks
{
    public interface IGameplayTaskOwnerInterface
    {
        bool IsValid();
        void OnGameplayTaskActivated(UGameplayTask Task);
        void OnGameplayTaskDeactivated(UGameplayTask Task);
        void OnGameplayTaskInitialized(UGameplayTask Task);
        UGameplayTasksComponent GetGameplayTasksComponent(UGameplayTask Task);
    }
}

