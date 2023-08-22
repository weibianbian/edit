using RailShootGame;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
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

        public AbilitySystemComponent()
        {
            SpawnedAttributes = new List<AttributeSet>();
            AbilityActorInfo = ReferencePool.Acquire<GameplayAbilityActorInfo>();
            ActiveGameplayEffects = new ActiveGameplayEffectsContainer();
        }
        public AttributeSet InitStats(Type Attributes)
        {
            AttributeSet AttributeObj = GetOrCreateAttributeSubobject(Attributes);
            return AttributeObj;
        }
        public void AddSpawnedAttribute(AttributeSet Attribute)
        {
            if (!SpawnedAttributes.Contains(Attribute))
            {
                SpawnedAttributes.Add(Attribute);
            }
        }
        public AttributeSet GetOrCreateAttributeSubobject(Type AttributeClass)
        {
            Actor OwningActor = GetOwner();
            AttributeSet MyAttributes = null;
            if (OwningActor != null && AttributeClass != null)
            {
                MyAttributes = GetAttributeSubobject(AttributeClass);
                if (MyAttributes == null)
                {
                    AttributeSet Attributes = ReferencePool.Acquire(AttributeClass) as AttributeSet;
                    AddSpawnedAttribute(Attributes);
                    MyAttributes = Attributes;
                }
            }
            return MyAttributes;
        }
        public AttributeSet GetAttributeSubobject(Type AttributeClass)
        {
            for (int i = 0; i < SpawnedAttributes.Count; i++)
            {
                if (SpawnedAttributes[i].GetType() == AttributeClass)
                {
                    return SpawnedAttributes[i];
                }
            }
            return null;
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
            //我们是否对此免疫
            ActiveGameplayEffect ImmunityGE = null;
            if (ActiveGameplayEffects.HasApplicationImmunityToSpec(Spec, ImmunityGE))
            {
                return new ActiveGameplayEffectHandle();
            }

            //确保我们在正确的位置创建规范的副本
            //我们在这里用INDEX_NONE初始化FActiveGameplayEffectHandle来处理即时GE的情况
            //像这样初始化它会将FActiveGameplayEffectHandle上的bPassedFiltersAndWasExecuted设置为true，这样我们就可以知道我们应用了GE
            ActiveGameplayEffectHandle MyHandle = new ActiveGameplayEffectHandle(-1);
            bool bTreatAsInfiniteDuration = Spec.Def.DurationPolicy == EGameplayEffectDurationType.Instant;
            bool bFoundExistingStackableGE = false;

            ActiveGameplayEffect AppliedEffect = new ActiveGameplayEffect();
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
                //if (OurCopyOfSpec.Def.RemoveGameplayEffectsWithTags)
                //{

                //}
                ExecuteGameplayEffect(OurCopyOfSpec);
            }
            //
            //ActiveGameplayEffects.AttemptRemoveActiveEffectsOnEffectApplication(OurCopyOfSpec, MyHandle);
            //
            for (int i = 0; i < Spec.TargetEffectSpecs.Count; i++)
            {
                ApplyGameplayEffectSpecToSelf(Spec.TargetEffectSpecs[i].Data);
            }
            AbilitySystemComponent InstigatorASC = Spec.GetContext().GetInstigatorAbilitySystemComponent();
            OnGameplayEffectAppliedToSelf(this, OurCopyOfSpec, MyHandle);
            if (InstigatorASC != null)
            {
                InstigatorASC.OnGameplayEffectAppliedToTarget(this, OurCopyOfSpec, MyHandle);
            }

            return MyHandle;
        }
        public void ExecuteGameplayEffect(GameplayEffectSpec Spec)
        {
            ActiveGameplayEffects.ExecuteActiveEffectsFrom(Spec);
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
        public void OnGameplayEffectAppliedToTarget(AbilitySystemComponent Target, GameplayEffectSpec SpecApplied, ActiveGameplayEffectHandle ActiveHandle)
        {

        }
        public void OnGameplayEffectAppliedToSelf(AbilitySystemComponent Source, GameplayEffectSpec SpecApplied, ActiveGameplayEffectHandle ActiveHandle)
        {

        }
    }
}

