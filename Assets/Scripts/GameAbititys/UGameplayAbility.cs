using RailShootGame;
using Sirenix.Utilities.Editor;
using System;
using Unity.VisualScripting;
using UnityEditor.PackageManager;

namespace GameplayAbilitySystem
{
    public class UGameplayAbility
    {
        //	The important functions:
        //	
        //		CanActivateAbility()	-   Const函数检查ability是否可激活。可由UI等调用
        //
        //		TryActivateAbility()	- 尝试激活该能力。调用CanActivateAbility()。输入事件可以直接调用它。.
        //								- 还处理每个执行的实例化逻辑和复制/预测调用.
        //		
        //		CallActivateAbility()	- Protected, non virtual function. Does some boilerplate 'pre activate' stuff, then calls ActivateAbility()
        //
        //		ActivateAbility()		- What the abilities *does*. This is what child classes want to override.
        //	
        //		CommitAbility()			- Commits reources/cooldowns etc. ActivateAbility() must call this!
        //		
        //		CancelAbility()			- Interrupts the ability (from an outside source).
        //
        //		EndAbility()			- The ability has ended. This is intended to be called by the ability to end itself.
        public CooldownGameplayEffect CooldownGameplayEffect { get; set; }
        public UGameplayEffect CostGameplayEffectClass;
        public FGameplayAbilityActorInfo CurrentActorInfo;

        /** For instanced abilities */
        public FGameplayAbilitySpecHandle CurrentSpecHandle;
        public virtual bool CanActivateAbility(FGameplayAbilitySpecHandle Handle, FGameplayAbilityActorInfo ActorInfo, FGameplayTagContainer SourceTags, FGameplayTagContainer TargetTags, FGameplayTagContainer OptionalRelevantTags)
        {
            UAbilitySystemComponent AbilitySystemComponent = ActorInfo.AbilitySystemComponent;
            if (AbilitySystemComponent == null)
            {
                return false;
            }

            UAbilitySystemGlobals AbilitySystemGlobals = UAbilitySystemGlobals.Get();
            if (!AbilitySystemGlobals.ShouldIgnoreCooldowns() && !CheckCooldown(Handle, ActorInfo))
            {
                return false;
            }
            if (!AbilitySystemGlobals.ShouldIgnoreCosts() && !CheckCost(Handle, ActorInfo))
            {
                return false;
            }
            FGameplayAbilitySpec Spec = AbilitySystemComponent.FindAbilitySpecFromHandle(Handle);
            if (Spec==null)
            {
                UnityEngine.Debug.LogError($"CanActivateAbility {this} failed, called with invalid Handle");
                return false;
            }
            return true;
        }
        public void CallActivateAbility(FGameplayAbilitySpecHandle Handle, FGameplayAbilityActorInfo ActorInfo, FGameplayAbilityActivationInfo ActivationInfo, FGameplayEventData TriggerEventData)
        {
            PreActivate(Handle);
            ActivateAbility(Handle, ActorInfo, ActivationInfo, TriggerEventData);
        }
        public void PreActivate(FGameplayAbilitySpecHandle Handle)
        {
        }
        public virtual void ActivateAbility(FGameplayAbilitySpecHandle Handle, FGameplayAbilityActorInfo ActorInfo, FGameplayAbilityActivationInfo ActivationInfo, FGameplayEventData TriggerEventData)
        {
            if (CommitAbility(Handle, ActorInfo, ActivationInfo))
            {

            }
        }
        public virtual void CancelAbility(FGameplayAbilitySpecHandle Handle)
        {

        }
        public virtual bool CommitAbility(FGameplayAbilitySpecHandle Handle, FGameplayAbilityActorInfo ActorInfo, FGameplayAbilityActivationInfo ActivationInfo)
        {
            //最后一次失败的机会(也许我们不再有资源提交，因为我们开始这个技能激活后)
            if (!CommitCheck(Handle, ActorInfo, ActivationInfo))
            {
                return false;
            }
            CommitExecute(Handle, ActorInfo);
            return true;
        }
        public void CommitExecute(FGameplayAbilitySpecHandle Handle, FGameplayAbilityActorInfo ActorInfo)
        {
            ApplyCooldown(Handle, ActorInfo);
            ApplyCost();
        }
        public bool CommitCheck(FGameplayAbilitySpecHandle Handle, FGameplayAbilityActorInfo ActorInfo, FGameplayAbilityActivationInfo ActivationInfo)
        {
            UAbilitySystemGlobals AbilitySystemGlobals = UAbilitySystemGlobals.Get();
            if (!AbilitySystemGlobals.ShouldIgnoreCooldowns() && !CheckCooldown(Handle, ActorInfo))
            {
                return false;
            }
            if (!AbilitySystemGlobals.ShouldIgnoreCosts() && !CheckCost(Handle, ActorInfo))
            {
                return false;
            }
            return true;
        }
        public bool CommitAbilityCooldown(FGameplayAbilitySpecHandle Handle, FGameplayAbilityActorInfo ActorInfo)
        {
            ApplyCooldown(Handle, ActorInfo);
            return true;
        }
        public bool CheckCooldown(FGameplayAbilitySpecHandle Handle, FGameplayAbilityActorInfo ActorInfo)
        {
            FGameplayTagContainer CooldownTags = GetCooldownTags();
            if (CooldownTags != null)
            {
                if (CooldownTags.Num() > 0)
                {
                    UAbilitySystemComponent AbilitySystemComponent = ActorInfo.AbilitySystemComponent;
                    if (AbilitySystemComponent.HasAnyMatchingGameplayTags(CooldownTags))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public bool CheckCost(FGameplayAbilitySpecHandle Handle, FGameplayAbilityActorInfo ActorInfo)
        {

            UGameplayEffect CostGE = GetCostGameplayEffect();
            if (CostGE != null)
            {
                UAbilitySystemComponent AbilitySystemComponent = ActorInfo.AbilitySystemComponent;
                if (!AbilitySystemComponent.CanApplyAttributeModifiers(CostGE, GetAbilityLevel(Handle, ActorInfo), MakeEffectContext(Handle, ActorInfo)))
                {
                    return false;
                }
            }
            return true;
        }
        public int GetAbilityLevel()
        {
            return GetAbilityLevel(CurrentSpecHandle, CurrentActorInfo);
        }
        public int GetAbilityLevel(FGameplayAbilitySpecHandle Handle, FGameplayAbilityActorInfo ActorInfo)
        {
            UAbilitySystemComponent AbilitySystemComponent = ActorInfo.AbilitySystemComponent;

            FGameplayAbilitySpec Spec = AbilitySystemComponent != null ? AbilitySystemComponent.FindAbilitySpecFromHandle(Handle) : null;

            if (Spec != null)
            {
                return Spec.Level;
            }
            return 1;
        }
        public FGameplayEffectContextHandle MakeEffectContext(FGameplayAbilitySpecHandle Handle, FGameplayAbilityActorInfo ActorInfo)
        {
            FGameplayEffectContextHandle Context = new FGameplayEffectContextHandle(new FGameplayEffectContext());
            // By default use the owner and avatar as the instigator and causer
            Context.AddInstigator(ActorInfo.OwnerActor, ActorInfo.AvatarActor);

            // add in the ability tracking here.
            Context.SetAbility(this);

            // Pass along the source object to the effect
            FGameplayAbilitySpec AbilitySpec = ActorInfo.AbilitySystemComponent.FindAbilitySpecFromHandle(Handle);
            if (AbilitySpec != null)
            {
                //Context.AddSourceObject(AbilitySpec.SourceObject);
            }

            return Context;
        }
        public FGameplayTagContainer GetCooldownTags()
        {
            UGameplayEffect CDGE = GetCooldownGameplayEffect();
            return CDGE != null ? CDGE.InheritableOwnedTagsContainer.CombinedTags : null;
        }
        public UGameplayEffect GetCostGameplayEffect()
        {
            if (CostGameplayEffectClass != null)
            {
                return CostGameplayEffectClass;
            }
            else
            {
                return null;
            }
        }
        public void ApplyCooldown(FGameplayAbilitySpecHandle Handle, FGameplayAbilityActorInfo ActorInfo)
        {
            UGameplayEffect CooldownGE = GetCooldownGameplayEffect();
            if (CooldownGE != null)
            {
                ApplyGameplayEffectToOwner(Handle, ActorInfo, CooldownGE, 1);
            }
        }
        public void ApplyCost()
        {

        }
        public CooldownGameplayEffect GetCooldownGameplayEffect()
        {
            return CooldownGameplayEffect;
        }
        public FActiveGameplayEffectHandle ApplyGameplayEffectToOwner(FGameplayAbilitySpecHandle Handle, FGameplayAbilityActorInfo ActorInfo, UGameplayEffect InGamepayEffect, float GameplayEffectLevel)
        {
            if (InGamepayEffect != null)
            {
                FGameplayEffectSpecHandle SpecHandle = MakeOutgoingGameplayEffectSpec(Handle, ActorInfo, InGamepayEffect, GameplayEffectLevel);
                if (SpecHandle != null)
                {
                    return ApplyGameplayEffectSpecToOwner(Handle, ActorInfo, SpecHandle);
                }
            }
            return new FActiveGameplayEffectHandle();
        }
        public FActiveGameplayEffectHandle ApplyGameplayEffectSpecToOwner(FGameplayAbilitySpecHandle AbilityHandle, FGameplayAbilityActorInfo ActorInfo, FGameplayEffectSpecHandle SpecHandle)
        {
            if (SpecHandle != null)
            {
                UAbilitySystemComponent AbilitySystemComponent = ActorInfo.AbilitySystemComponent;
                return AbilitySystemComponent.ApplyGameplayEffectSpecToSelf(SpecHandle.Data);
            }
            return new FActiveGameplayEffectHandle();
        }
        public FGameplayEffectSpecHandle MakeOutgoingGameplayEffectSpec(FGameplayAbilitySpecHandle Handle, FGameplayAbilityActorInfo ActorInfo, UGameplayEffect InGamepayEffect, float GameplayEffectLevel)
        {
            UAbilitySystemComponent AbilitySystemComponent = ActorInfo.AbilitySystemComponent;
            FGameplayEffectSpecHandle NewHandle = AbilitySystemComponent.MakeOutgoingSpec(null, GameplayEffectLevel, null);
            return null;
        }
    }
}

