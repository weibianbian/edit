using RailShootGame;

namespace GameplayAbilitySystem
{
    public interface IGameplayCueInterface
    {
        void HandleGameplayCue(Actor TargetActor, GameplayTag GameplayCueTag, EGameplayCueEvent EventType, GameplayCueParameters Parameters);
    }
}

