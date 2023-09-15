using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Rendering;

namespace GameplayAbilitySystem
{
    public class UGameplayEffect
    {
        public EGameplayEffectDurationType DurationPolicy;
        public EGameplayEffectStackingType StackingType;
        public EGameplayEffectStackingExpirationPolicy StackExpirationPolicy;
        public EGameplayEffectStackingDurationPolicy StackDurationRefreshPolicy;
        public EGameplayEffectStackingPeriodPolicy StackPeriodResetPolicy;
        public List<FGameplayModifierInfo> Modifiers = new List<FGameplayModifierInfo>();
        public List<GameplayCue> GameplayCues = new List<GameplayCue>();
        public FInheritedTagContainer RemoveGameplayEffectsWithTags = new FInheritedTagContainer();
        public FInheritedTagContainer InheritableGameplayEffectTags = new FInheritedTagContainer();
        public FInheritedTagContainer InheritableOwnedTagsContainer = new FInheritedTagContainer();
        public FGameplayEffectModifierMagnitude DurationMagnitude;
        public FGameplayTagRequirements OngoingTagRequirements;
        public FGameplayTagRequirements GrantedApplicationImmunityTags;
        //赋予匹配此查询的游戏特效免疫。查询功能更强大，但比GrantedApplicationImmunityTags稍慢。
        public FGameplayEffectQuery GrantedApplicationImmunityQuery;
        public FScalableFloat Period;
        public float Duration;
        public int StackLimitCount;
        public bool bDenyOverflowApplication = false;
        public bool bClearStackOnOverflow = false;
        /*如果为true，效果在应用程序上执行，然后在每个周期间隔执行。如果为false，则在第一个周期结束之前不会执行。*/
        public bool bExecutePeriodicEffectOnApplication;

        public UGameplayEffect()
        {
            DurationPolicy = EGameplayEffectDurationType.Instant;
            Period = new FScalableFloat(0);
            OngoingTagRequirements = new FGameplayTagRequirements();
            bExecutePeriodicEffectOnApplication = true;
        }
    }
}

