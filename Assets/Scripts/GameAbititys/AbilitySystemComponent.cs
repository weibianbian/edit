using RailShootGame;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public class AbilitySystemComponent : ActorComponent
    {
        public GameplayAbilitySpecContainer ActivatableAbilities;
        public ActiveGameplayEffectsContainer ActiveGameplayEffects;
        public GameplayTagCountContainer GameplayTagCountContainer;
        public List<AttributeSet> SpawnedAttributes;
        public GameplayAbilityActorInfo AbilityActorInfo;

        public AbilitySystemComponent(Actor owner) : base(owner)
        {

        }
        public void InitStats(AttributeSet Attributes)
        {
            AddSpawnedAttribute(Attributes);
        }
        public void AddSpawnedAttribute(AttributeSet Attribute)
        {
            if (!SpawnedAttributes.Contains(Attribute))
            {
                SpawnedAttributes.Add(Attribute);
            }
        }
        public T GetSet<T>() where T : AttributeSet
        {
            for (int i = 0; i < SpawnedAttributes.Count; i++)
            {
                if (SpawnedAttributes[i] is T)
                {
                    return SpawnedAttributes[i] as T;
                }
            }
            return null;
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
            GameplayEffectContextHandle Context = MakeEffectContext();
            GameplayEffectSpec Spec = new GameplayEffectSpec(InGameplayEffect, Context, InLevel);
            ActiveGameplayEffectHandle ret = ApplyGameplayEffectSpecToTarget(Spec, InTarget);
            return ret;
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
            ActiveGameplayEffect ImmunityGE = null;
            if (ActiveGameplayEffects.HasApplicationImmunityToSpec(Spec, ImmunityGE))
            {
                return new ActiveGameplayEffectHandle();
            }


            ActiveGameplayEffectHandle MyHandle;
            ActiveGameplayEffectHandle ReturnHandle = new ActiveGameplayEffectHandle();
            ActiveGameplayEffect AppliedEffect = new ActiveGameplayEffect();
            bool bFoundExistingStackableGE = false;
            bool bTreatAsInfiniteDuration = Spec.Def.DurationPolicy == EGameplayEffectDurationType.Instant;
            bool bInvokeGameplayCueApplied = Spec.Def.DurationPolicy != EGameplayEffectDurationType.Instant;
            GameplayEffectSpec StackSpec = null;
            GameplayEffectSpec OurCopyOfSpec = null;
            if (Spec.Def.DurationPolicy != EGameplayEffectDurationType.Instant || bTreatAsInfiniteDuration)
            {
                AppliedEffect = ActiveGameplayEffects.ApplyGameplayEffectSpec(Spec, ref bFoundExistingStackableGE);
                if (AppliedEffect == null)
                {
                    return new ActiveGameplayEffectHandle();
                }
                MyHandle = AppliedEffect.Handle;
                OurCopyOfSpec = AppliedEffect.Spec;
            }
            if (OurCopyOfSpec == null)
            {
                StackSpec = Spec;
                OurCopyOfSpec = StackSpec;
            }

            for (int i = 0; i < Spec.Def.Modifiers.Count; i++)
            {

            }
            if (bTreatAsInfiniteDuration)
            {

            }
            else if (Spec.Def.DurationPolicy == EGameplayEffectDurationType.Instant)
            {

            }

            return ReturnHandle;
        }
        public void CheckDurationExpired(ActiveGameplayEffectHandle Handle)
        {
            ActiveGameplayEffects.CheckDuration(Handle);
        }
        public GameplayEffectContextHandle MakeEffectContext()
        {
            GameplayEffectContextHandle Context = new GameplayEffectContextHandle(new GameplayEffectContext());

            Context.AddInstigator(AbilityActorInfo.OwnerActor, AbilityActorInfo.AvatarActor);
            return Context;

        }
        public GameplayEffectSpecHandle MakeOutgoingSpec(GameplayEffect InGameplayEffect, float Level, GameplayEffectContextHandle Context)
        {
            GameplayEffectSpec NewSpec = new GameplayEffectSpec(InGameplayEffect, Context, Level);
            //传递给投掷物，投掷物击中到目标后被应用
            return new GameplayEffectSpecHandle(NewSpec);
        }
        public bool InternalTryActivateAbility(GameplayAbilitySpecHandle Handle)
        {
            return true;
        }
        public Action<GameplayTag, int> RegisterGameplayTagEvent(GameplayTag Tag, EGameplayTagEventType EventType)
        {
            return GameplayTagCountContainer.RegisterGameplayTagEvent(Tag, EventType);
        }
        public void UnregisterGameplayTagEvent(GameplayTag Tag, EGameplayTagEventType EventType)
        {
            Action<GameplayTag, int> ret = GameplayTagCountContainer.RegisterGameplayTagEvent(Tag, EventType);
        }
    }
}

