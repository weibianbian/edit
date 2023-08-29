using RailShootGame;
using System;
using UnityEngine;

namespace GameplayAbilitySystem
{
    public class AbilitySystemTestAttributeSet : AttributeSet
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

        public override void PreAttributeBaseChange(GameplayAttribute Attribute, float NewValue)
        {
            base.PreAttributeBaseChange(Attribute, NewValue);

            ClampAttribute(Attribute, ref NewValue);
        }
        public override void PostAttributeBaseChange(GameplayAttribute Attribute, float OldValue, float NewValue)
        {
            base.PostAttributeBaseChange(Attribute, OldValue, NewValue);

        }
        void ClampAttribute(GameplayAttribute Attribute, ref float NewValue)
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
        GameplayAttribute GetHealthAttribute()
        {
            GameplayAttribute ret = new GameplayAttribute();
            ret.SetUProperty("Health", typeof(AbilitySystemTestAttributeSet));
            return ret;
        }

    }
}

