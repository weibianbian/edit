using RailShootGame;

namespace GameplayAbilitySystem
{
    public class GameplayCueParameters
    {

    }
    public class GameplayCueManager
    {
        public virtual void InvokeGameplayCueExecuted_FromSpec(AbilitySystemComponent OwningComponent, GameplayEffectSpec Spec)
        {

        }
        public virtual void HandleGameplayCues(Actor TargetActor, GameplayTagContainer GameplayCueTags, EGameplayCueEvent EventType, GameplayCueParameters Parameters)
        {
            for (int i = 0; i < GameplayCueTags.GameplayTags.Count; i++)
            {
                HandleGameplayCues(TargetActor, GameplayCueTags.GameplayTags[i], EventType, Parameters);
            }
        }
        public virtual void HandleGameplayCues(Actor TargetActor, GameplayTag GameplayCueTag, EGameplayCueEvent EventType, GameplayCueParameters Parameters)
        {
            RouteGameplayCue(TargetActor, GameplayCueTag, EventType, Parameters);
        }
        public virtual void RouteGameplayCue(Actor TargetActor, GameplayTag GameplayCueTag, EGameplayCueEvent EventType, GameplayCueParameters Parameters)
        {
            IGameplayCueInterface GameplayCueInterface = TargetActor as IGameplayCueInterface;

            GameplayCueInterface.HandleGameplayCue(TargetActor, GameplayCueTag, EventType, Parameters);
        }
        public void OnCreated()
        {

        }
    }
}

