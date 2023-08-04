using System.Collections.Generic;
using System.Xml.Xsl;
using UnityEditor;

namespace GameplayAbilitySystem
{
    public enum EGameplayEffectDurationType
    {
        /** This effect applies instantly */
        Instant,
        /** This effect lasts forever */
        Infinite,
        /** The duration of this effect will be specified by a magnitude */
        HasDuration
    }
    public class GameplayEffect
    {
        public EGameplayEffectDurationType DurationPolicy;
        public List<GameplayModifierInfo> Modifiers = new List<GameplayModifierInfo>();
        public List<GameplayCue> GameplayCues = new List<GameplayCue>();
        public float Period;
        public float Duration;

        public GameplayEffect()
        {
            DurationPolicy = EGameplayEffectDurationType.Instant;
        }
    }
    public class GameplayEffectSpec
    {
        public GameplayEffect Def;
        public float Duration;
        public float Level;
        private GameplayEffectContextHandle EffectContext;
        public GameplayEffectSpec(GameplayEffect InDef, GameplayEffectContextHandle InEffectContext, float InLevel)
        {
            Initialize(InDef, InEffectContext, InLevel);
        }
        public void Initialize(GameplayEffect InDef, GameplayEffectContextHandle InEffectContext, float InLevel)
        {
            Def = InDef;
            Level = InLevel;
            SetContext(InEffectContext);
            SetLevel(InLevel);
        }
        public void SetContext(GameplayEffectContextHandle NewEffectContext)
        {
            EffectContext = NewEffectContext;
        }
        public void SetLevel(float InLevel)
        {
            Level = InLevel;
        }
        public float GetDuration()
        {
            return Duration;
        }
        public float CalculateModifiedDuration()
        {
            return 0;
        }
    }
    public class GameplayEffectSpecHandle
    {
        public GameplayEffectSpec Data;
        public GameplayEffectSpecHandle(GameplayEffectSpec InData)
        {
            Data = InData;
        }
    }
    public class GameplayEffectContext
    {

    }
    public class GameplayEffectContextHandle
    {
        public GameplayEffectContext Data;
        public GameplayEffectContextHandle(GameplayEffectContext InData)
        {
            Data = InData;
        }
    }
    public class GameplayCue { }

}

