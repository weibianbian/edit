using RailShootGame;
using System;
using UnityEngine;

namespace GameplayAbilitySystem
{
    public class AbilitySystemTestAttributeSet : UAttributeSet
    {
        public GameplayAttributeData MaxHealth;
        public GameplayAttributeData Health;
        public GameplayAttributeData Mana;
        public GameplayAttributeData MaxMana;
        public GameplayAttributeData Damage;
        public GameplayAttributeData SpellDamage;
        public GameplayAttributeData PhysicalDamage;
        public GameplayAttributeData CritChance;
        public GameplayAttributeData CritMultiplier;
        public GameplayAttributeData ArmorDamageReduction;
        public GameplayAttributeData DodgeChance;
        public GameplayAttributeData LifeSteal;
        public GameplayAttributeData Strength;
        public GameplayAttributeData StackingAttribute1;
        public GameplayAttributeData StackingAttribute2;
        public GameplayAttributeData NoStackAttribute;
        public override void PreAttributeChange(FGameplayAttribute Attribute, float NewValue)
        {
            base.PreAttributeChange(Attribute, NewValue);

            ClampAttribute(Attribute, ref NewValue);
        }
        public override void PostAttributeChange(FGameplayAttribute Attribute, float OldValue, float NewValue)
        {
            base.PostAttributeChange(Attribute, OldValue, NewValue);
        }
        public override void PreAttributeBaseChange(FGameplayAttribute Attribute, float NewValue)
        {
            base.PreAttributeBaseChange(Attribute, NewValue);

            ClampAttribute(Attribute, ref NewValue);
        }
        public override void PostAttributeBaseChange(FGameplayAttribute Attribute, float OldValue, float NewValue)
        {
            base.PostAttributeBaseChange(Attribute, OldValue, NewValue);

        }
        void ClampAttribute(FGameplayAttribute Attribute, ref float NewValue)
        {
            if (Attribute == GetHealthAttribute())
            {
                // Do not allow health to go negative or above max health.
                NewValue = Mathf.Clamp(NewValue, 0.0f, MaxHealth.BaseValue);
            }
            //else if (Attribute == GetMaxHealthAttribute())
            //{
            //    // Do not allow max health to drop below 1.
            //    NewValue = FMath::Max(NewValue, 1.0f);
            //}
        }
        FGameplayAttribute GetHealthAttribute()
        {
            FGameplayAttribute ret = new FGameplayAttribute();
            ret.SetUProperty(typeof(AbilitySystemTestAttributeSet).GetField("Health"), typeof(AbilitySystemTestAttributeSet));
            return ret;
        }

    }
}

