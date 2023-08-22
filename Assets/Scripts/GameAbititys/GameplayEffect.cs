using RailShootGame;
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
        public EGameplayEffectStackingType StackingType;
        public List<GameplayModifierInfo> Modifiers = new List<GameplayModifierInfo>();
        public List<GameplayCue> GameplayCues = new List<GameplayCue>();
        public FInheritedTagContainer RemoveGameplayEffectsWithTags = new FInheritedTagContainer();

        public float Period;
        public float Duration;
        public int StackLimitCount;
        public bool bDenyOverflowApplication = false;
        public bool bClearStackOnOverflow = false;

        public GameplayEffect()
        {
            DurationPolicy = EGameplayEffectDurationType.Instant;
        }
    }
    public class FModifierSpec
    {

    }
    public class GameplayEffectSpec
    {
        public GameplayEffect Def;
        public float Duration;
        public float Level;
        private GameplayEffectContextHandle EffectContext;
        public int StackCount;
        public List<GameplayEffectSpecHandle> TargetEffectSpecs = new List<GameplayEffectSpecHandle>();
        public List<FModifierSpec> Modifiers = new List<FModifierSpec>();
        public GameplayEffectSpec(GameplayEffect InDef, GameplayEffectContextHandle InEffectContext, float InLevel)
        {
            Initialize(InDef, InEffectContext, InLevel);
        }
        public GameplayEffectSpec(GameplayEffectSpec Other)
        {

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
        public GameplayEffectContextHandle GetContext()
        {
            return EffectContext;
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
        public void CalculateModifierMagnitudes()
        {
            for (int ModIdx = 0; ModIdx < Modifiers.Count; ModIdx++)
            {
                GameplayModifierInfo ModDef = Def.Modifiers[ModIdx];
                FModifierSpec ModSpec = Modifiers[ModIdx];
                if (ModDef.ModifierMagnitude.AttemptCalculateMagnitude())
                {

                }
            }
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
        public Actor Instigator;
        public Actor EffectCauser;
        public AbilitySystemComponent InstigatorAbilitySystemComponent;
        public void AddInstigator(Actor InInstigator, Actor InEffectCauser)
        {
            Instigator = InInstigator;
            EffectCauser = InEffectCauser;
        }
        public AbilitySystemComponent GetInstigatorAbilitySystemComponent()
        {
            return InstigatorAbilitySystemComponent;
        }
    }
    public class GameplayEffectContextHandle
    {
        public GameplayEffectContext Data;
        public GameplayEffectContextHandle(GameplayEffectContext InData)
        {
            Data = InData;
        }
        public void AddInstigator(Actor InInstigator, Actor InEffectCauser)
        {
            Data.AddInstigator(InInstigator, InEffectCauser);
        }
        public AbilitySystemComponent GetInstigatorAbilitySystemComponent()
        {
            return Data.GetInstigatorAbilitySystemComponent();
        }
    }
    public class GameplayCue { }
}

