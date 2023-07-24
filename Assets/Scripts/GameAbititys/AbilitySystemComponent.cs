using RailShootGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayAbilitySystem
{
    public class AbilitySystemComponent : ActorCompt
    {
        public GameplayAbilitySpecContainer ActivatableAbilities;
        public AbilitySystemComponent(Actor owner) : base(owner)
        {

        }
        public void GiveAbility(GameplayAbilitySpec AbilitySpec)
        {
            ActivatableAbilities.items.Add(AbilitySpec);
            //ÐèÒª¸´ÖÆ
            OnGiveAbility(AbilitySpec);
        }
        public void OnGiveAbility(GameplayAbilitySpec Spec)
        {

        }
        public override void TickComponent()
        {

        }

    }
}

