using UEngine.GameFramework;
using UEngine.GameplayTags;

namespace UEngine.GameplayAbilities
{
    public interface IGameplayCueInterface
    {
        void HandleGameplayCue(AActor TargetActor, FGameplayTag GameplayCueTag, EGameplayCueEvent EventType, GameplayCueParameters Parameters);
    }
}

