using GameplayAbilitySystem;

namespace RailShootGame
{
    public class AbilitySystemTestActor : Actor, IGameplayCueInterface, IAbilitySystemInterface
    {
        private AbilitySystemComponent AbilitySystemComponent;
        public AbilitySystemComponent GetAbilitySystemComponent()
        {
            return AbilitySystemComponent;
        }
        public AbilitySystemTestActor()
        {
            AbilitySystemComponent = new AbilitySystemComponent(this);
        }
        public void PostInitializeComponents()
        {
            AbilitySystemComponent.InitStats(new AbilitySystemTestAttributeSet());
        }

        public void HandleGameplayCue(Actor TargetActor, GameplayTag GameplayCueTag, EGameplayCueEvent EventType, GameplayCueParameters Parameters)
        {

        }
    }
}

