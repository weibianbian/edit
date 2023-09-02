﻿using System.Collections.Generic;

namespace GameplayAbilitySystem
{
    public class GameplayAbilitySpecContainer
    {
        public List<GameplayAbilitySpec> items;
        UAbilitySystemComponent owner;

        public void RegisterWithOwner(UAbilitySystemComponent owner)
        {
            this.owner = owner;
        }

    }

}

