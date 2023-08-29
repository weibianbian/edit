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
            AbilitySystemComponent = ReferencePool.Acquire<AbilitySystemComponent>();
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

