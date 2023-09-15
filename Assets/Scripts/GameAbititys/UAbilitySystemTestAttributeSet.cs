using RailShootGame;
using System;
using UnityEngine;

namespace GameplayAbilitySystem
{
    public class UAbilitySystemTestAttributeSet : UAttributeSet
    {
        public FGameplayAttributeData MaxHealth;
        public FGameplayAttributeData Health;
        public FGameplayAttributeData Mana;
        public FGameplayAttributeData MaxMana;
        public FGameplayAttributeData Damage;
        public FGameplayAttributeData SpellDamage;
        public FGameplayAttributeData PhysicalDamage;
        public FGameplayAttributeData CritChance;
        public FGameplayAttributeData CritMultiplier;
        public FGameplayAttributeData ArmorDamageReduction;
        public FGameplayAttributeData DodgeChance;
        public FGameplayAttributeData LifeSteal;
        public FGameplayAttributeData Strength;
        public FGameplayAttributeData StackingAttribute1;
        public FGameplayAttributeData StackingAttribute2;
        public FGameplayAttributeData NoStackAttribute;
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
            ret.SetUProperty(typeof(UAbilitySystemTestAttributeSet).GetField("Health"), typeof(UAbilitySystemTestAttributeSet));
            return ret;
        }

    }
}

