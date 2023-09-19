using RailShootGame;

namespace GameplayAbilitySystem
{
    public interface IGameplayCueInterface
    {
        void HandleGameplayCue(Actor TargetActor, FGameplayTag GameplayCueTag, EGameplayCueEvent EventType, GameplayCueParameters Parameters);
    }
}

