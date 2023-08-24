using Core.Timer;
using System.Collections.Generic;

namespace GameplayAbilitySystem
{
    public static class GlobalActiveGameplayEffectHandles
    {
        public static Dictionary<ActiveGameplayEffectHandle, AbilitySystemComponent> Map = new Dictionary<ActiveGameplayEffectHandle, AbilitySystemComponent>();
    }
    public class ActiveGameplayEffectHandle
    {
        public int Handle;
        private bool bPassedFiltersAndWasExecuted;
        public ActiveGameplayEffectHandle(int InHandle)
        {
            Handle = InHandle;
            bPassedFiltersAndWasExecuted = true;
        }
        public ActiveGameplayEffectHandle()
        {
            Handle = -1;
            bPassedFiltersAndWasExecuted = false;
        }
        public static ActiveGameplayEffectHandle GenerateNewHandle(AbilitySystemComponent OwningComponent)
        {
            ActiveGameplayEffectHandle NewHandle = new ActiveGameplayEffectHandle();
            GlobalActiveGameplayEffectHandles.Map.Add(NewHandle, OwningComponent);
            return NewHandle;
        }
    }
    public class ActiveGameplayEffect
    {
        public GameplayEffectSpec Spec;
        public ActiveGameplayEffectHandle Handle;
        public TimerHandle DurationHandle;
        public bool IsPendingRemove;

        public float GetDuration()
        {
            return Spec.GetDuration();
        }

    }
}
