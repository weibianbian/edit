using UEngine.Core;
using UEngine.GameFramework;
using UEngine.GameplayAbilities;
using UEngine.GameplayTags;

namespace RailShootGame
{
    public class AbilitySystemTestActor : AActor, IGameplayCueInterface, IAbilitySystemInterface
    {
        private UAbilitySystemComponent AbilitySystemComponent;
        public UAbilitySystemComponent GetAbilitySystemComponent()
        {
            return AbilitySystemComponent;
        }
        public AbilitySystemTestActor()
        {
            AbilitySystemComponent = ReferencePool.Acquire<UAbilitySystemComponent>();
            AbilitySystemComponent.SetOwner(this);
            AbilitySystemComponent.PostInitProperties();
        }
        public override void PostInitializeComponents()
        {
            AbilitySystemComponent.InitStats(typeof(UAbilitySystemTestAttributeSet));
        }

        public void HandleGameplayCue(AActor TargetActor, FGameplayTag GameplayCueTag, EGameplayCueEvent EventType, GameplayCueParameters Parameters)
        {
        }
    }
}

