using System.Collections.Generic;

namespace UEngine.GameplayAbilities
{
    public class GameplayAbilitySpecContainer
    {
        public List<FGameplayAbilitySpec> items=new List<FGameplayAbilitySpec>();
        UAbilitySystemComponent owner;

        public void RegisterWithOwner(UAbilitySystemComponent owner)
        {
            this.owner = owner;
        }

    }

}

