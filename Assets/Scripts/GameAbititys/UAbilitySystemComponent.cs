using RailShootGame;
using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UIElements;

namespace GameplayAbilitySystem
{

    public class UAbilitySystemComponent : UGameplayTasksComponent
    {
        public GameplayAbilitySpecContainer ActivatableAbilities;
        public FActiveGameplayEffectsContainer ActiveGameplayEffects;
        public FGameplayTagCountContainer GameplayTagCountContainer;
        public List<UAttributeSet> SpawnedAttributes;
        public FGameplayAbilityActorInfo AbilityActorInfo;
        public List<FGameplayAbilitySpecHandle> InputPressedSpecHandles;
        public List<FGameplayAbilitySpecHandle> InputHeldSpecHandles;
        public static List<FGameplayAbilitySpecHandle> AbilitiesToActivate;
        public AActor OwnerActor;
        public UAbilitySystemComponent()
        {
            SpawnedAttributes = new List<UAttributeSet>();
            AbilityActorInfo = ReferencePool.Acquire<FGameplayAbilityActorInfo>();
            ActiveGameplayEffects = new FActiveGameplayEffectsContainer();
            GameplayTagCountContainer = new FGameplayTagCountContainer();
            AbilitiesToActivate = new List<FGameplayAbilitySpecHandle>();
            InputPressedSpecHandles = new List<FGameplayAbilitySpecHandle>();
            InputHeldSpecHandles = new List<FGameplayAbilitySpecHandle>();
            ActivatableAbilities = new GameplayAbilitySpecContainer();
        }
        public override void InitializeComponent()
        {
            base.InitializeComponent();
            AActor Owner = GetOwner();
            InitAbilityActorInfo(Owner, Owner);
        }
        public virtual void InitAbilityActorInfo(AActor InOwnerActor, AActor InAvatarActor)
        {
            AbilityActorInfo.InitFromActor(InOwnerActor, InAvatarActor, this);

            SetOwnerActor(InOwnerActor);
        }
        public void SetOwnerActor(AActor NewOwnerActor)
        {
            OwnerActor = NewOwnerActor;
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
        public void AbilityInputTagPressed(FGameplayTag InputTag)
        {
            for (int i = 0; i < ActivatableAbilities.items.Count; i++)
            {
                FGameplayAbilitySpec AbilitySpec = ActivatableAbilities.items[i];
                if (AbilitySpec.Ability != null && AbilitySpec.DynamicAbilityTags.HasTagExact(InputTag))
                {
                    InputPressedSpecHandles.Add(AbilitySpec.Handle);
                    InputHeldSpecHandles.Add(AbilitySpec.Handle);
                }

            }
        }
        public void ProcessAbilityInput(float DeltaTime)
        {
            AbilitiesToActivate.Clear();
            for (int i = 0; i < InputPressedSpecHandles.Count; i++)
            {
                FGameplayAbilitySpecHandle SpecHandle = InputPressedSpecHandles[i];
                FGameplayAbilitySpec AbilitySpec = FindAbilitySpecFromHandle(SpecHandle);
                if (AbilitySpec != null && AbilitySpec.Ability != null)
                {
                    AbilitySpec.InputPressed = true;
                    if (AbilitySpec.IsActive())
                    {
                        AbilitySpecInputPressed(AbilitySpec);
                    }
                    else
                    {
                        AbilitiesToActivate.Add(AbilitySpec.Handle);
                    }
                }
            }
            for (int i = 0; i < AbilitiesToActivate.Count; i++)
            {
                TryActivateAbility(AbilitiesToActivate[i]);
            }
            InputPressedSpecHandles.Clear();
            InputHeldSpecHandles.Clear();
        }
        public void NotifyAbilityEnded(FGameplayAbilitySpecHandle Handle, UGameplayAbility Ability, bool bWasCancelled)
        {
            FGameplayAbilitySpec Spec = FindAbilitySpecFromHandle(Handle);
            if (Spec == null)
            {
                // The ability spec may have been removed while we were ending. We can assume everything was cleaned up if the spec isnt here.
                return;
            }
        }
        public void ExecutePeriodicEffect(FActiveGameplayEffectHandle Handle)
        {
            ActiveGameplayEffects.ExecutePeriodicGameplayEffect(Handle);
        }
        public UAttributeSet GetOrCreateAttributeSubobject(Type AttributeClass)
        {
            AActor OwningActor = GetOwner();
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
        public bool TryActivateAbility(FGameplayAbilitySpecHandle AbilityToActivate)
        {
            FGameplayAbilitySpec Spec = FindAbilitySpecFromHandle(AbilityToActivate);
            if (Spec == null)
            {
                Debug.LogError("TryActivateAbility called with invalid Handle");
                return false;
            }
            UGameplayAbility Ability = Spec.Ability;
            if (Ability == null)
            {
                Debug.LogError("TryActivateAbility called with invalid Handle");
                return false;
            }
            return InternalTryActivateAbility(AbilityToActivate, null);
        }
        public FGameplayAbilitySpec FindAbilitySpecFromHandle(FGameplayAbilitySpecHandle Handle)
        {
            for (int i = 0; i < ActivatableAbilities.items.Count; i++)
            {
                if (ActivatableAbilities.items[i].Handle == Handle)
                {
                    return ActivatableAbilities.items[i];
                }
            }
            return FGameplayAbilitySpec.Default;
        }
        public void GiveAbility(FGameplayAbilitySpec AbilitySpec)
        {
            ActivatableAbilities.items.Add(AbilitySpec);
            //需要复制
            OnGiveAbility(AbilitySpec);
        }
        public void OnGiveAbility(FGameplayAbilitySpec Spec)
        {
            Spec.Ability.OnGiveAbility(AbilityActorInfo, Spec);
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
        public void OnRemoveAbility(FGameplayAbilitySpec Spec)
        {

        }
        public void AbilitySpecInputPressed(FGameplayAbilitySpec Spec)
        {
            Spec.InputPressed = true;
            if (Spec.IsActive())
            {
                //Spec.Ability.
            }
        }
        public override void TickComponent(float DeltaTime)
        {

        }
        public bool CanApplyAttributeModifiers(UGameplayEffect GameplayEffect, float Level, FGameplayEffectContextHandle EffectContext)
        {
            return ActiveGameplayEffects.CanApplyAttributeModifiers(GameplayEffect, Level, EffectContext);
        }
        public OnGameplayAttributeValueChange GetGameplayAttributeValueChangeDelegate(FGameplayAttribute Attribute)
        {
            return ActiveGameplayEffects.GetGameplayAttributeValueChangeDelegate(Attribute);
        }
        public FActiveGameplayEffectHandle ApplyGameplayEffectToTarget(UGameplayEffect InGameplayEffect, UAbilitySystemComponent InTarget, float InLevel)
        {
            FGameplayEffectContextHandle Context = MakeEffectContext();
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
                OnImmunityBlockGameplayEffect(Spec, ImmunityGE);
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
        public void OnImmunityBlockGameplayEffect(FGameplayEffectSpec Spec, FActiveGameplayEffect ImmunityGE)
        {

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
        public FGameplayEffectContextHandle MakeEffectContext()
        {
            FGameplayEffectContextHandle Context = new FGameplayEffectContextHandle(new FGameplayEffectContext());

            Context.AddInstigator(AbilityActorInfo.OwnerActor, AbilityActorInfo.AvatarActor);
            return Context;

        }
        public FGameplayEffectSpecHandle MakeOutgoingSpec(UGameplayEffect InGameplayEffect, float Level, FGameplayEffectContextHandle Context)
        {
            FGameplayEffectSpec NewSpec = new FGameplayEffectSpec(InGameplayEffect, Context, Level);
            //传递给投掷物，投掷物击中到目标后被应用
            return new FGameplayEffectSpecHandle(NewSpec);
        }
        public bool InternalTryActivateAbility(FGameplayAbilitySpecHandle Handle, FGameplayEventData TriggerEventData)
        {
            FGameplayAbilitySpec Spec = FindAbilitySpecFromHandle(Handle);
            if (Spec == null)
            {
                Debug.LogError($"InternalTryActivateAbility called with a valid handle but no matching ability was found. Handle");
                return false;
            }
            UGameplayAbility Ability = Spec.Ability;

            if (Ability == null)
            {
                Debug.LogError(("InternalTryActivateAbility called with invalid Ability"));
                return false;
            }
            FGameplayTagContainer SourceTags = null;
            FGameplayTagContainer TargetTags = null;
            if (TriggerEventData != null)
            {
                SourceTags = TriggerEventData.InstigatorTags;
                TargetTags = TriggerEventData.TargetTags;
            }
            FGameplayTagContainer InternalTryActivateAbilityFailureTags = new FGameplayTagContainer();
            if (!Ability.CanActivateAbility(Handle, AbilityActorInfo, SourceTags, TargetTags, InternalTryActivateAbilityFailureTags))
            {
                return false;
            }
            Spec.ActivationInfo = new FGameplayAbilityActivationInfo();
            FGameplayAbilityActivationInfo ActivationInfo = Spec.ActivationInfo;
            Ability.CallActivateAbility(Handle, AbilityActorInfo, ActivationInfo, TriggerEventData);
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
        public virtual bool HasAnyMatchingGameplayTags(FGameplayTagContainer TagContainer)
        {
            return GameplayTagCountContainer.HasAnyMatchingGameplayTags(TagContainer);
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

