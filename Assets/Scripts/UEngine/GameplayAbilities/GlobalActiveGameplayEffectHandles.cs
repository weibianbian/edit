using System.Collections.Generic;

namespace UEngine.GameplayAbilities
{
    public static class GlobalActiveGameplayEffectHandles
    {
        public static Dictionary<FActiveGameplayEffectHandle, UAbilitySystemComponent> Map = new Dictionary<FActiveGameplayEffectHandle, UAbilitySystemComponent>();
    }
}
