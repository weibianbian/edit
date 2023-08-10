using RailShootGame;
using Sirenix.Utilities.Editor;
using System;
using System.Collections.Generic;
using static UnityEngine.UI.GridLayoutGroup;

namespace GameplayAbilitySystem
{
    public class ActiveGameplayEffectsContainer : List<ActiveGameplayEffect>
    {
        AbilitySystemComponent Owner;
        public Dictionary<GameplayAttribute, OnGameplayAttributeValueChange> AttributeValueChangeDelegates;
        public List<GameplayEffect> ApplicationImmunityQueryEffects;

        public ActiveGameplayEffect ApplyGameplayEffectSpec(GameplayEffectSpec Spec, ref bool bFoundExistingStackableGE)
        {
            bFoundExistingStackableGE = false;
            ActiveGameplayEffect AppliedActiveGE = null;
            ActiveGameplayEffect ExistingStackableGE = FindStackableActiveGameplayEffect(Spec);

            bool bSetDuration = true;
            bool bSetPeriod = true;
            int StartingStackCount = 0;
            int NewStackCount = 0;

            if (ExistingStackableGE != null)
            {
                bFoundExistingStackableGE = true;
                GameplayEffectSpec ExistingSpec = ExistingStackableGE.Spec;
                StartingStackCount = ExistingSpec.StackCount;

                if (ExistingSpec.StackCount == ExistingSpec.Def.StackLimitCount)
                {
                    if (!HandleActiveGameplayEffectStackOverflow(ExistingStackableGE, ExistingSpec, Spec))
                    {
                        return null;
                    }
                }
                NewStackCount = ExistingSpec.StackCount + Spec.StackCount;
                if (ExistingSpec.Def.StackLimitCount > 0)
                {
                    NewStackCount = Math.Min(NewStackCount, ExistingSpec.Def.StackLimitCount);
                }
                ExistingStackableGE.Spec = Spec;
                ExistingStackableGE.Spec.StackCount = NewStackCount;
            }
            else
            {
                ActiveGameplayEffectHandle NewHandle = ActiveGameplayEffectHandle.GenerateNewHandle(Owner);
                AppliedActiveGE = new ActiveGameplayEffect()
                {
                    Handle = NewHandle,
                    Spec = Spec,
                };
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
                    if (Owner != null && bSetDuration)
                    {
                        TimerManager TimerManager = new TimerManager();
                        TimerManager.SetTimer(ref AppliedActiveGE.DurationHandle, TimerDelegate<AbilitySystemComponent, ActiveGameplayEffectHandle>.Create((@owner, @handle) =>
                        {
                            @owner.CheckDurationExpired(@handle);
                        }, Owner, AppliedActiveGE.Handle), FinalDuration, false);
                    }
                }
                return new ActiveGameplayEffect();
            }
            return null;
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
            for (int ActiveGEIdx = 0; ActiveGEIdx < this.Count; ++ActiveGEIdx)
            {
                ActiveGameplayEffect Effect = this[ActiveGEIdx];
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
        public ActiveGameplayEffect FindStackableActiveGameplayEffect(GameplayEffectSpec Spec)
        {
            ActiveGameplayEffect StackableGE = null;
            GameplayEffect GEDef = Spec.Def;
            EGameplayEffectStackingType StackingType = GEDef.StackingType;
            if ((StackingType != EGameplayEffectStackingType.None) && (GEDef.DurationPolicy != EGameplayEffectDurationType.Instant))
            {
                AbilitySystemComponent SourceASC = Spec.GetContext().GetInstigatorAbilitySystemComponent();
                for (int i = 0; i < this.Count; i++)
                {
                    ActiveGameplayEffect ActiveEffect = this[i];
                    if (ActiveEffect.Spec.Def == Spec.Def &&
                        ((StackingType == EGameplayEffectStackingType.AggregateByTarget) || (SourceASC != null && (SourceASC == ActiveEffect.Spec.GetContext().GetInstigatorAbilitySystemComponent()))))
                    {
                        StackableGE = ActiveEffect;
                        break;
                    }
                }
            }
            return StackableGE;
        }
        public bool HandleActiveGameplayEffectStackOverflow(ActiveGameplayEffect ActiveStackableGE, GameplayEffectSpec OldSpec, GameplayEffectSpec OverflowingSpec)
        {
            GameplayEffect StackedGE = OldSpec.Def;
            bool bAllowOverflowApplication = !(StackedGE.bDenyOverflowApplication);

            if (!bAllowOverflowApplication && StackedGE.bClearStackOnOverflow)
            {
                //Owner.RemoveActiveGameplayEffect(ActiveStackableGE.Handle);
            }
            return bAllowOverflowApplication;
        }
    }

}

