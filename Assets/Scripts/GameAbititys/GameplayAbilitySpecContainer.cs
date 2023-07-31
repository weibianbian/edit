using System.Collections.Generic;

namespace GameplayAbilitySystem
{
    public class GameplayAbilitySpecContainer
    {
        public List<GameplayAbilitySpec> items;
        AbilitySystemComponent owner;

        public void RegisterWithOwner(AbilitySystemComponent owner)
        {
            this.owner = owner;
        }

    }

}

