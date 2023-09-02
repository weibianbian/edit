using System.Collections.Generic;

namespace GameplayAbilitySystem
{
    public static class GlobalActiveGameplayEffectHandles
    {
        public static Dictionary<FActiveGameplayEffectHandle, UAbilitySystemComponent> Map = new Dictionary<FActiveGameplayEffectHandle, UAbilitySystemComponent>();
    }
}
