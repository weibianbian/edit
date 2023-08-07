﻿using RailShootGame;
using System.Collections.Generic;

namespace GameplayAbilitySystem
{

    public class ActiveGameplayEffectsContainer
    {
        AbilitySystemComponent owner;
        public List<ActiveGameplayEffect> GameplayEffects_Internal;
        public Dictionary<GameplayAttribute, OnGameplayAttributeValueChange> AttributeValueChangeDelegates;
        public List<GameplayEffect> ApplicationImmunityQueryEffects;

        public ActiveGameplayEffect ApplyGameplayEffectSpec(GameplayEffectSpec Spec, ref bool bFoundExistingStackableGE)
        {
            ActiveGameplayEffect AppliedActiveGE = null;
            ActiveGameplayEffectHandle NewHandle = ActiveGameplayEffectHandle.GenerateNewHandle(owner);

            AppliedActiveGE = new ActiveGameplayEffect()
            {
                Handle = NewHandle,
                Spec = Spec,
            };
            bool bSetDuration = true;
            GameplayEffectSpec AppliedEffectSpec = AppliedActiveGE.Spec;
            float DurationBaseValue = AppliedEffectSpec.GetDuration();
            if (DurationBaseValue > 0)
            {
                float FinalDuration = AppliedEffectSpec.CalculateModifiedDuration();
                if (FinalDuration <= 0.0f)
                {
                    FinalDuration = 0.1f;
                }
                //AppliedEffectSpec.SetDuration(FinalDuration, true);
                if (owner != null && bSetDuration)
                {
                    TimerManager TimerManager = new TimerManager();
                    TimerManager.SetTimer(ref AppliedActiveGE.DurationHandle, TimerDelegate<AbilitySystemComponent, ActiveGameplayEffectHandle>.Create((@owner, @handle) =>
                    {
                        @owner.CheckDurationExpired(@handle);
                    }, owner, AppliedActiveGE.Handle), FinalDuration, false);
                }
            }
            return new ActiveGameplayEffect();
        }
        public OnGameplayAttributeValueChange GetGameplayAttributeValueChangeDelegate(GameplayAttribute Attribute)
        {
            if (!AttributeValueChangeDelegates.TryGetValue(Attribute, out OnGameplayAttributeValueChange value))
            {
                value = new OnGameplayAttributeValueChange();
                AttributeValueChangeDelegates.Add(Attribute, value);
            }
            return value;
        }
        public void CheckDuration(ActiveGameplayEffectHandle Handle)
        {
            for (int ActiveGEIdx = 0; ActiveGEIdx < GameplayEffects_Internal.Count; ++ActiveGEIdx)
            {
                ActiveGameplayEffect Effect = GameplayEffects_Internal[ActiveGEIdx];
                if (Effect.Handle == Handle)
                {
                    if (Effect.IsPendingRemove)
                    {
                        break;
                    }
                }
                TimerManager TimerManager = null;
                float Duration = Effect.GetDuration();
                //float CurrentTime = GetWorldTime();
            }
        }
        public bool HasApplicationImmunityToSpec(GameplayEffectSpec SpecToApply, ActiveGameplayEffect OutGEThatProvidedImmunity)
        {
            for (int i = 0; i < ApplicationImmunityQueryEffects.Count; i++)
            {
                GameplayEffect EffectDef = ApplicationImmunityQueryEffects[i];

            }
            return true;
        }
    }

}

