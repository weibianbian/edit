using RailShootGame;
using System;
using System.Collections.Generic;
using UnityEditor.Events;
using UnityEngine;

namespace GameplayAbilitySystem
{

    public class UAbilitySystemComponent : ActorComponent
    {
        public GameplayAbilitySpecContainer ActivatableAbilities;
        public FActiveGameplayEffectsContainer ActiveGameplayEffects;
        public FGameplayTagCountContainer GameplayTagCountContainer;
        public List<UAttributeSet> SpawnedAttributes;
        public GameplayAbilityActorInfo AbilityActorInfo;

        public UAbilitySystemComponent()
        {
            SpawnedAttributes = new List<UAttributeSet>();
            AbilityActorInfo = ReferencePool.Acquire<GameplayAbilityActorInfo>();
            ActiveGameplayEffects = new FActiveGameplayEffectsContainer();
            GameplayTagCountContainer = new FGameplayTagCountContainer();
        }

        public override void OnRegister()
        {
            ActiveGameplayEffects.RegisterWithOwner(this);
        }
        public UAttributeSet InitStats(Type Attributes)
        {
            UAttributeSet AttributeObj = GetOrCreateAttributeSubobject(Attributes);
            return AttributeObj;
        }
        public void AddSpawnedAttribute(UAttributeSet Attribute)
        {
            if (!SpawnedAttributes.Contains(Attribute))
            {
                SpawnedAttributes.Add(Attribute);
            }
        }
        public void ExecutePeriodicEffect(FActiveGameplayEffectHandle Handle)
        {
            ActiveGameplayEffects.ExecutePeriodicGameplayEffect(Handle);
        }
        public UAttributeSet GetOrCreateAttributeSubobject(Type AttributeClass)
        {
            Actor OwningActor = GetOwner();
            UAttributeSet MyAttributes = null;
            if (OwningActor != null && AttributeClass != null)
            {
                MyAttributes = GetAttributeSubobject(AttributeClass);
                if (MyAttributes == null)
                {
                    UAttributeSet Attributes = ReferencePool.Acquire(AttributeClass) as UAttributeSet;
                    AddSpawnedAttribute(Attributes);
                    MyAttributes = Attributes;
                }
            }
            return MyAttributes;
        }
        public UAttributeSet GetAttributeSubobject(Type AttributeClass)
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

        public T GetSet<T>() where T : UAttributeSet
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
        public virtual void GetOwnedGameplayTags(FGameplayTagContainer TagContainer)
        {
            TagContainer.Reset();

            TagContainer.AppendTags(GameplayTagCountContainer.GetExplicitGameplayTags());
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
        public OnGameplayAttributeValueChange GetGameplayAttributeValueChangeDelegate(FGameplayAttribute Attribute)
        {
            return ActiveGameplayEffects.GetGameplayAttributeValueChangeDelegate(Attribute);
        }
        public FActiveGameplayEffectHandle ApplyGameplayEffectToTarget(UGameplayEffect InGameplayEffect, UAbilitySystemComponent InTarget, float InLevel)
        {
            GameplayEffectContextHandle Context = MakeEffectContext();
            FGameplayEffectSpec Spec = new FGameplayEffectSpec(InGameplayEffect, Context, InLevel);
            FActiveGameplayEffectHandle ret = ApplyGameplayEffectSpecToTarget(Spec, InTarget);
            return ret;
        }
        public FActiveGameplayEffectHandle ApplyGameplayEffectSpecToTarget(FGameplayEffectSpec Spec, UAbilitySystemComponent InTarget)
        {
            FActiveGameplayEffectHandle ReturnHandle = new FActiveGameplayEffectHandle();
            if (InTarget != null)
            {
                ReturnHandle = InTarget.ApplyGameplayEffectSpecToSelf(Spec);
            }
            return ReturnHandle;
        }
        public FActiveGameplayEffectHandle ApplyGameplayEffectSpecToSelf(FGameplayEffectSpec Spec)
        {
            //ActiveGameplayEffectsContainer.ApplyGameplayEffectSpec
            //我们是否对此免疫
            FActiveGameplayEffect ImmunityGE = null;
            if (ActiveGameplayEffects.HasApplicationImmunityToSpec(Spec, ImmunityGE))
            {
                return new FActiveGameplayEffectHandle();
            }
            //检查特效是否成功应用
            float ChanceToApply = Spec.GetChanceToApplyToTarget();
            if (ChanceToApply > 1.0f - 1E-8f)
            {
                return new FActiveGameplayEffectHandle();
            }
            //确保我们在正确的位置创建规范的副本
            //我们在这里用INDEX_NONE初始化FActiveGameplayEffectHandle来处理即时GE的情况
            //像这样初始化它会将FActiveGameplayEffectHandle上的bPassedFiltersAndWasExecuted设置为true，这样我们就可以知道我们应用了GE
            FActiveGameplayEffectHandle MyHandle = new FActiveGameplayEffectHandle(-1);
            bool bFoundExistingStackableGE = false;

            FActiveGameplayEffect AppliedEffect = new FActiveGameplayEffect();
            //在可能将预测即时效果修改为无限持续效果之前，现在将其缓存
            bool bInvokeGameplayCueApplied = Spec.Def.DurationPolicy != EGameplayEffectDurationType.Instant;
            FGameplayEffectSpec OurCopyOfSpec = null;
            FGameplayEffectSpec StackSpec = null;
            if (Spec.Def.DurationPolicy != EGameplayEffectDurationType.Instant)
            {
                AppliedEffect = ActiveGameplayEffects.ApplyGameplayEffectSpec(Spec, ref bFoundExistingStackableGE);
                if (AppliedEffect == null)
                {
                    return new FActiveGameplayEffectHandle();
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
            if (Spec.Def.DurationPolicy == EGameplayEffectDurationType.Instant)
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
            UAbilitySystemComponent InstigatorASC = Spec.GetContext().GetInstigatorAbilitySystemComponent();
            OnGameplayEffectAppliedToSelf(this, OurCopyOfSpec, MyHandle);
            if (InstigatorASC != null)
            {
                InstigatorASC.OnGameplayEffectAppliedToTarget(this, OurCopyOfSpec, MyHandle);
            }

            return MyHandle;
        }
        public bool HasAttributeSetForAttribute(FGameplayAttribute Attribute)
        {
            //return (Attribute.IsValid() && (Attribute.IsSystemAttribute() || GetAttributeSubobject(Attribute.GetAttributeSetClass()) != nullptr));
            return true;
        }
        public void ExecuteGameplayEffect(FGameplayEffectSpec Spec)
        {
            ActiveGameplayEffects.ExecuteActiveEffectsFrom(Spec);
        }
        public void CheckDurationExpired(FActiveGameplayEffectHandle Handle)
        {
            ActiveGameplayEffects.CheckDuration(Handle);
        }
        public float GetNumericAttributeBase(FGameplayAttribute Attribute)
        {
            //if (Attribute.IsSystemAttribute())
            //{
            //    return 0.0f;
            //}

            return ActiveGameplayEffects.GetAttributeBaseValue(Attribute);
        }
        public GameplayEffectContextHandle MakeEffectContext()
        {
            GameplayEffectContextHandle Context = new GameplayEffectContextHandle(new GameplayEffectContext());

            Context.AddInstigator(AbilityActorInfo.OwnerActor, AbilityActorInfo.AvatarActor);
            return Context;

        }
        public GameplayEffectSpecHandle MakeOutgoingSpec(UGameplayEffect InGameplayEffect, float Level, GameplayEffectContextHandle Context)
        {
            FGameplayEffectSpec NewSpec = new FGameplayEffectSpec(InGameplayEffect, Context, Level);
            //传递给投掷物，投掷物击中到目标后被应用
            return new GameplayEffectSpecHandle(NewSpec);
        }
        public bool InternalTryActivateAbility(GameplayAbilitySpecHandle Handle)
        {
            return true;
        }
        public Action<FGameplayTag, int> RegisterGameplayTagEvent(FGameplayTag Tag, EGameplayTagEventType EventType)
        {
            return GameplayTagCountContainer.RegisterGameplayTagEvent(Tag, EventType);
        }
        public void UnregisterGameplayTagEvent(FGameplayTag Tag, EGameplayTagEventType EventType)
        {
            Action<FGameplayTag, int> ret = GameplayTagCountContainer.RegisterGameplayTagEvent(Tag, EventType);
        }
        public void OnGameplayEffectAppliedToTarget(UAbilitySystemComponent Target, FGameplayEffectSpec SpecApplied, FActiveGameplayEffectHandle ActiveHandle)
        {

        }
        public void OnGameplayEffectAppliedToSelf(UAbilitySystemComponent Source, FGameplayEffectSpec SpecApplied, FActiveGameplayEffectHandle ActiveHandle)
        {

        }
        public float GetNumericAttribute(FGameplayAttribute Attribute)
        {
            UAttributeSet AttributeSetOrNull = null;
            Type AttributeSetClass = Attribute.AttributeOwner;
            if (AttributeSetClass != null && AttributeSetClass.IsSubclassOf(typeof(UAttributeSet)))
            {
                AttributeSetOrNull = GetAttributeSubobject(AttributeSetClass);
            }
            if (AttributeSetOrNull == null)
            {
                return 0;
            }
            return Attribute.GetNumericValue(AttributeSetOrNull);
        }
        public void SetNumericAttribute_Internal(FGameplayAttribute Attribute, float NewFloatValue)
        {
            UAttributeSet AttributeSet = null;
            Type AttributeSetClass = Attribute.AttributeOwner;
            if (AttributeSetClass != null && AttributeSetClass.IsSubclassOf(typeof(UAttributeSet)))
            {
                AttributeSet = GetAttributeSubobject(AttributeSetClass);
            }
            Attribute.SetNumericValueChecked(NewFloatValue, AttributeSet);
        }
        public bool RemoveActiveGameplayEffect(FActiveGameplayEffectHandle Handle, int StacksToRemove = -1)
        {
            return ActiveGameplayEffects.RemoveActiveGameplayEffect(Handle, StacksToRemove);
        }
    }
}

