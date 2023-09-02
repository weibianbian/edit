using GameplayAbilitySystem;

namespace RailShootGame
{
    public class AbilitySystemTestActor : Actor, IGameplayCueInterface, IAbilitySystemInterface
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
            AbilitySystemComponent.InitStats(typeof(AbilitySystemTestAttributeSet));
        }

        public void HandleGameplayCue(Actor TargetActor, GameplayTag GameplayCueTag, EGameplayCueEvent EventType, GameplayCueParameters Parameters)
        {

        }
    }
}

