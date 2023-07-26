using RailShootGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayAbilitySystem
{
    public class AbilitySystemComponent : ActorCompt
    {
        public GameplayAbilitySpecContainer ActivatableAbilities;
        public List<AttributeSet> SpawnedAttributes;
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
        public void ClearAbility(int id)
        {
            for (int idx = 0; idx < ActivatableAbilities.items.Count; idx++)
            {
                if (ActivatableAbilities.items[idx].id == id)
                {
                    OnRemoveAbility(ActivatableAbilities.items[idx]);
                    ActivatableAbilities.items.RemoveAt(idx);
                    return;
                }
            }
        }
        public void OnRemoveAbility(GameplayAbilitySpec Spec)
        {

        }
        public void AbilitySpecInputPressed(GameplayAbilitySpec Spec)
        {
            Spec.InputPressed = true;
            if (Spec.IsActive())
            {

            }
        }
        public override void TickComponent()
        {

        }

    }
}

