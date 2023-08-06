using RailShootGame;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayAbilitySystem
{
    //AbilitySystemComponent
    //              ---TryActivateAbility
    //GameplayAbility
    //              ---CanActivateAbility
    //GameplayAbility
    //              ---CallActivateAbility
    //GameplayAbility
    //              ---K2_ActivateAbility
    //GameplayAbility
    //              ---CommitAbility
    //执行蓝图AbilityTask
    //              ---PlayMontageTask-------Wati-------GameplayAbility(EndAbility)
    //执行事件（动画轴上的事件）
    //              ---SendGameplayEventToActor
    //GameplayAbility
    //              ---ApplyGameplayEffectToTarget
    public struct GameplayAbilitySpecHandle
    {

        int Handle;

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

        public static bool operator ==(GameplayAbilitySpecHandle a, GameplayAbilitySpecHandle b)
        {
            return a.Handle == b.Handle;
        }
        public static bool operator !=(GameplayAbilitySpecHandle a, GameplayAbilitySpecHandle b)
        {
            return a.Handle != b.Handle;
        }

    }
    public struct GameplayAttribute
    {

    }

    public class AbilitySystemComponent : ActorCompt
    {
        public GameplayAbilitySpecContainer ActivatableAbilities;
        public ActiveGameplayEffectsContainer ActiveGameplayEffects;
        public List<AttributeSet> SpawnedAttributes;
        public AbilitySystemComponent(Actor owner) : base(owner)
        {

        }
        public bool TryActiveAbility(GameplayAbilitySpecHandle AbilityToActivate)
        {
            GameplayAbilitySpec Spec = FindAbilitySpecFromHandle(AbilityToActivate);
            if (Spec == null)
            {
                Debug.LogError("TryActivateAbility called with invalid Handle");
                return false;
            }
            GameplayAbility Ability = Spec.Ability;
            if (Ability == null)
            {
                Debug.LogError("TryActivateAbility called with invalid Handle");
                return false;
            }
            return true;
        }
        public GameplayAbilitySpec FindAbilitySpecFromHandle(GameplayAbilitySpecHandle Handle)
        {
            for (int i = 0; i < ActivatableAbilities.items.Count; i++)
            {
                if (ActivatableAbilities.items[i].Handle == Handle)
                {
                    return ActivatableAbilities.items[i];
                }
            }
            return GameplayAbilitySpec.Default;
        }
        public void GiveAbility(GameplayAbilitySpec AbilitySpec)
        {
            ActivatableAbilities.items.Add(AbilitySpec);
            //需要复制
            OnGiveAbility(AbilitySpec);
        }
        public void OnGiveAbility(GameplayAbilitySpec Spec)
        {

        }
        public void ClearAbility(int id)
        {
            for (int idx = 0; idx < ActivatableAbilities.items.Count; idx++)
            {
                if (ActivatableAbilities.items[idx].id == id)
                {
                    OnRemoveAbility(ActivatableAbilities.items[idx]);
                    ActivatableAbilities.items.RemoveAt(idx);
                    return;
                }
            }
        }
        public void OnRemoveAbility(GameplayAbilitySpec Spec)
        {

        }
        public void AbilitySpecInputPressed(GameplayAbilitySpec Spec)
        {
            Spec.InputPressed = true;
            if (Spec.IsActive())
            {

            }
        }
        public override void TickComponent()
        {

        }
        public OnGameplayAttributeValueChange GetGameplayAttributeValueChangeDelegate(GameplayAttribute Attribute)
        {
            return ActiveGameplayEffects.GetGameplayAttributeValueChangeDelegate(Attribute);
        }
        public ActiveGameplayEffectHandle ApplyGameplayEffectToTarget(GameplayEffect InGameplayEffect, AbilitySystemComponent InTarget, float InLevel)
        {
            return new ActiveGameplayEffectHandle();
        }
        public ActiveGameplayEffectHandle ApplyGameplayEffectSpecToTarget(GameplayEffectSpec Spec, AbilitySystemComponent InTarget)
        {
            ActiveGameplayEffectHandle ReturnHandle = new ActiveGameplayEffectHandle();
            if (InTarget != null)
            {
                ReturnHandle = InTarget.ApplyGameplayEffectSpecToSelf(Spec);
            }
            return ReturnHandle;
        }
        public ActiveGameplayEffectHandle ApplyGameplayEffectSpecToSelf(GameplayEffectSpec Spec)
        {
            //ActiveGameplayEffectsContainer.ApplyGameplayEffectSpec
                //
            ActiveGameplayEffectHandle ReturnHandle = new ActiveGameplayEffectHandle();
            ActiveGameplayEffect AppliedEffect = new ActiveGameplayEffect();
            if (Spec.Def.DurationPolicy != EGameplayEffectDurationType.Instant)
            {
                AppliedEffect = ActiveGameplayEffects.ApplyGameplayEffectSpec(Spec);
                return new ActiveGameplayEffectHandle();

            }

            for (int i = 0; i < Spec.Def.Modifiers.Count; i++)
            {

            }

            if (Spec.Def.DurationPolicy == EGameplayEffectDurationType.Instant)
            {

            }
            return ReturnHandle;
        }
        public void CheckDurationExpired(ActiveGameplayEffectHandle Handle)
        {
            ActiveGameplayEffects.CheckDuration(Handle);
        }
        
        public GameplayEffectSpecHandle MakeOutgoingSpec(GameplayEffect InGameplayEffect,float Level, GameplayEffectContextHandle Context)
        {
            GameplayEffectSpec NewSpec = new GameplayEffectSpec(InGameplayEffect, Context, Level);
            //传递给投掷物，投掷物击中到目标后被应用
            return new GameplayEffectSpecHandle(NewSpec);
        }
    }
}

