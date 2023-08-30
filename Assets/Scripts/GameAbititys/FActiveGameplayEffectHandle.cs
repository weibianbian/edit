using Core.Timer;
using System.Collections.Generic;

namespace GameplayAbilitySystem
{
    public static class GlobalActiveGameplayEffectHandles
    {
        public static Dictionary<FActiveGameplayEffectHandle, AbilitySystemComponent> Map = new Dictionary<FActiveGameplayEffectHandle, AbilitySystemComponent>();
    }
    public class FActiveGameplayEffectHandle
    {
        public int Handle;
        private bool bPassedFiltersAndWasExecuted;
        public FActiveGameplayEffectHandle(int InHandle)
        {
            Handle = InHandle;
            bPassedFiltersAndWasExecuted = true;
        }
        public FActiveGameplayEffectHandle()
        {
            Handle = -1;
            bPassedFiltersAndWasExecuted = false;
        }
        public static FActiveGameplayEffectHandle GenerateNewHandle(AbilitySystemComponent OwningComponent)
        {
            FActiveGameplayEffectHandle NewHandle = new FActiveGameplayEffectHandle();
            GlobalActiveGameplayEffectHandles.Map.Add(NewHandle, OwningComponent);
            return NewHandle;
        }
    }
    public class FActiveGameplayEffect
    {
        public FGameplayEffectSpec Spec;
        public FActiveGameplayEffectHandle Handle;
        public TimerHandle DurationHandle;
        public bool IsPendingRemove;

        public float GetDuration()
        {
            return Spec.GetDuration();
        }

    }
}
