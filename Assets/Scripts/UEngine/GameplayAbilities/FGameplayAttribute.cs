﻿using RailShootGame;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;

namespace UEngine.GameplayAbilities
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
            FGameplayAttributeData DataPtr = (FGameplayAttributeData)fi.GetValue(Src);
            return DataPtr.GetCurrentValue();
        }
        public void SetNumericValueChecked(float NewValue, UAttributeSet Dest)
        {
            float OldValue = 0.0f;
            FieldInfo fi = (Attribute);
            FGameplayAttributeData DataPtr = (FGameplayAttributeData)fi.GetValue(Dest);
            OldValue = DataPtr.GetCurrentValue();
            Dest.PreAttributeChange(this, NewValue);
            DataPtr.SetCurrentValue(NewValue);
            Dest.PostAttributeChange(this, OldValue, NewValue);
        }
        public float GetNumericValueChecked(UAttributeSet Src)
        {
            FieldInfo fi = (Attribute);
            FGameplayAttributeData DataPtr = (FGameplayAttributeData)fi.GetValue(Src);
            {
                return DataPtr.GetCurrentValue();
            }
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

