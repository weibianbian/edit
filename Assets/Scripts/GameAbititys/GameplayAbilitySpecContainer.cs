using System.Collections.Generic;

namespace GameplayAbilitySystem
{
    public class GameplayAbilitySpecContainer
    {
        public List<FGameplayAbilitySpec> items;
        UAbilitySystemComponent owner;

        public void RegisterWithOwner(UAbilitySystemComponent owner)
        {
            this.owner = owner;
        }

    }

}

