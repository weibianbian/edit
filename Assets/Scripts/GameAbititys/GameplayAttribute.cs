using RailShootGame;
using System;
using System.Reflection;

namespace GameplayAbilitySystem
{
    public class GameplayAttribute
    {
        public Type AttributeOwner;
        public string Attribute;

        public void SetUProperty(string NewProperty, Type InAttributeOwner)
        {
            Attribute = NewProperty;
            AttributeOwner = InAttributeOwner;
        }
        public FieldInfo GetUProperty()
        {
            return AttributeOwner.GetField(Attribute);
        }
        public float GetNumericValue(AttributeSet Src)
        {
            FieldInfo fi = AttributeOwner.GetField(Attribute);
            GameplayAttributeData DataPtr = (GameplayAttributeData)fi.GetValue(Src);
            return DataPtr.GetCurrentValue();
        }
        public void SetNumericValueChecked(float NewValue, AttributeSet Dest)
        {
            float OldValue = 0.0f;
            FieldInfo fi = AttributeOwner.GetField(Attribute);
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

