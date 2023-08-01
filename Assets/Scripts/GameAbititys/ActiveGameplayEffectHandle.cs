using GameplayAbilitySystem;
using System.Collections.Generic;

namespace GameplayAbilitySystem
{
    public static class GlobalActiveGameplayEffectHandles
    {
        public static Dictionary<ActiveGameplayEffectHandle, AbilitySystemComponent> Map;
    }
    public class ActiveGameplayEffectHandle
    {
        public int Handle;
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

        
    }
}

