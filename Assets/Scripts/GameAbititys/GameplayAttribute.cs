using RailShootGame;
using System;
using System.Reflection;

namespace GameplayAbilitySystem
{
    public class GameplayAttribute
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
        public float GetNumericValue(AttributeSet Src)
        {
            FieldInfo fi = (Attribute);
            GameplayAttributeData DataPtr = (GameplayAttributeData)fi.GetValue(Src);
            return DataPtr.GetCurrentValue();
        }
        public void SetNumericValueChecked(float NewValue, AttributeSet Dest)
        {
            float OldValue = 0.0f;
            FieldInfo fi = (Attribute);
            GameplayAttributeData DataPtr = (GameplayAttributeData)fi.GetValue(Dest);
            OldValue = DataPtr.GetCurrentValue();
            Dest.PreAttributeChange(this, NewValue);
            DataPtr.SetCurrentValue(NewValue);

            fi.SetValue(Dest, DataPtr);
            Dest.PostAttributeChange(this, OldValue, NewValue);
        }
        public static bool operator ==(GameplayAttribute a, GameplayAttribute b)
        {
            return a.Attribute == b.Attribute;
        }
        public static bool operator !=(GameplayAttribute a, GameplayAttribute b)
        {
            return a.Attribute != b.Attribute;
        }

    }
}

