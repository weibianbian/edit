using RailShootGame;

namespace GameplayAbilitySystem
{
    public interface IGameplayCueInterface
    {
        void HandleGameplayCue(AActor TargetActor, FGameplayTag GameplayCueTag, EGameplayCueEvent EventType, GameplayCueParameters Parameters);
    }
}

