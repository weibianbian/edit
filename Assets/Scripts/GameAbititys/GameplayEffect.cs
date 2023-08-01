using System.Collections.Generic;

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
        public List<GameplayCue> GameplayCues=new List<GameplayCue>();
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

        public float GetDuration()
        {
            return Duration;
        }
        public float CalculateModifiedDuration()
        {
            return 0;
        }
    }
    public class GameplayCue { }

}

