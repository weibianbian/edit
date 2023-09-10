using RailShootGame;
using System;
using System.Diagnostics;
using System.Reflection;

namespace GameplayAbilitySystem
{
    public class FGameplayAttribute
    {
        public Type AttributeOwner;
        public FieldInfo Attribute;

        public void SetUProperty(FieldInfo NewProperty, Type InAttributeOwner)
        {
            Attribute = NewProperty;
            AttributeOwner = InAttributeOwner;
        }
        public FieldInfo GetUProperty()
        {
            return (Attribute);
        }
        public float GetNumericValue(UAttributeSet Src)
        {
            FieldInfo fi = (Attribute);
            GameplayAttributeData DataPtr = (GameplayAttributeData)fi.GetValue(Src);
            return DataPtr.GetCurrentValue();
        }
        public void SetNumericValueChecked(float NewValue, UAttributeSet Dest)
        {
            float OldValue = 0.0f;
            FieldInfo fi = (Attribute);
            GameplayAttributeData DataPtr = (GameplayAttributeData)fi.GetValue(Dest);
            OldValue = DataPtr.GetCurrentValue();
            Dest.PreAttributeChange(this, NewValue);
            fi.SetValue(Dest, DataPtr);
            UnityEngine.Debug.Log($"SetNumericValueChecked=NewValue={NewValue}");
            Dest.PostAttributeChange(this, OldValue, NewValue);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public static bool operator ==(FGameplayAttribute a, FGameplayAttribute b)
        {
            return a.Attribute == b.Attribute;
        }
        public static bool operator !=(FGameplayAttribute a, FGameplayAttribute b)
        {
            return a.Attribute != b.Attribute;
        }

    }
}

