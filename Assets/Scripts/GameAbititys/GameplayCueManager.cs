using RailShootGame;
using System.Collections.Generic;

namespace GameplayAbilitySystem
{
    public class GameplayCueParameters
    {

    }
    public class FGameplayCuePendingExecute
    {

    }
    public class GameplayCueManager
    {
        public List<FGameplayCuePendingExecute> PendingExecuteCues = new List<FGameplayCuePendingExecute>();
        public virtual void InvokeGameplayCueExecuted_FromSpec(UAbilitySystemComponent OwningComponent, FGameplayEffectSpec Spec)
        {
            if (Spec.Def.GameplayCues.Count == 0)
            {
                return;
            }
            FGameplayCuePendingExecute PendingCue = new FGameplayCuePendingExecute();

        }
        public void AddPendingCueExecuteInternal(FGameplayCuePendingExecute PendingCue)
        {
            if (ProcessPendingCueExecute(PendingCue))
            {
                PendingExecuteCues.Add(PendingCue); 
            }
            FlushPendingCues();
        }
        public virtual void FlushPendingCues()
        {
            List<FGameplayCuePendingExecute> LocalPendingExecuteCues = PendingExecuteCues;
            for (int i = 0; i < LocalPendingExecuteCues.Count; i++)
            {
                FGameplayCuePendingExecute PendingCue = LocalPendingExecuteCues[i];

            }
        }
        public virtual bool ProcessPendingCueExecute(FGameplayCuePendingExecute PendingCue)
        {
            return true;
        }
        public virtual void HandleGameplayCues(Actor TargetActor, FGameplayTagContainer GameplayCueTags, EGameplayCueEvent EventType, GameplayCueParameters Parameters)
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

