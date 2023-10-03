using RailShootGame;
using System;

namespace GameplayAbilitySystem
{
    public class FGameplayEffectContext
    {
        public int AbilityLevel;
        public Actor Instigator;
        public Actor EffectCauser;
        public UAbilitySystemComponent InstigatorAbilitySystemComponent;
        public void AddInstigator(Actor InInstigator, Actor InEffectCauser)
        {
            Instigator = InInstigator;
            EffectCauser = InEffectCauser;
        }
        public UAbilitySystemComponent GetInstigatorAbilitySystemComponent()
        {
            return InstigatorAbilitySystemComponent;
        }
        public void SetAbility(UGameplayAbility InGameplayAbility)
        {
            if (InGameplayAbility != null)
            {
                AbilityLevel = InGameplayAbility.GetAbilityLevel();
            }
        }
    }
}

