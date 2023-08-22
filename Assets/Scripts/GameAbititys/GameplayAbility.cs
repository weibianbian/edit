﻿using RailShootGame;
using UnityEditor.PackageManager;

namespace GameplayAbilitySystem
{
    public class CooldownGameplayEffect : GameplayEffect
    {

    }
    public class GameplayAbilityActorInfo : ReferencePoolObject
    {
        public Actor OwnerActor;
        public Actor AvatarActor;
        public AbilitySystemComponent AbilitySystemComponent;

    }
    public class GameplayAbility
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
        public virtual bool CanActivateAbility(GameplayAbilitySpecHandle Handle)
        {
            return false;
        }
        public void CallActivateAbility(GameplayAbilitySpecHandle Handle)
        {
            PreActivate(Handle);
            //ActivateAbility(Handle, null);
        }
        public void PreActivate(GameplayAbilitySpecHandle Handle)
        {
        }
        public virtual void ActivateAbility(GameplayAbilitySpecHandle Handle, GameplayAbilityActorInfo ActorInfo, Character owner)
        {
            if (CommitAbility(Handle, ActorInfo))
            {

            }
        }
        public virtual void CancelAbility(GameplayAbilitySpecHandle Handle)
        {

        }
        public virtual bool CommitAbility(GameplayAbilitySpecHandle Handle, GameplayAbilityActorInfo ActorInfo)
        {
            CommitExecute(Handle, ActorInfo);
            return false;
        }
        public void CommitExecute(GameplayAbilitySpecHandle Handle, GameplayAbilityActorInfo ActorInfo)
        {
            ApplyCooldown(Handle, ActorInfo);
            ApplyCost();
        }
        public bool CommitAbilityCooldown(GameplayAbilitySpecHandle Handle, GameplayAbilityActorInfo ActorInfo)
        {
            ApplyCooldown(Handle, ActorInfo);
            return true;
        }
        public void ApplyCooldown(GameplayAbilitySpecHandle Handle, GameplayAbilityActorInfo ActorInfo)
        {
            GameplayEffect CooldownGE = GetCooldownGameplayEffect();
            if (CooldownGE != null)
            {
                //ApplyGameplayEffectToOwner(Handle, ActorInfo, CooldownGE, GetAbilityLevel(Handle, ActorInfo));
            }
        }
        public void ApplyCost()
        {

        }
        public CooldownGameplayEffect GetCooldownGameplayEffect()
        {
            return CooldownGameplayEffect;
        }
        public void ApplyGameplayEffectToOwner(GameplayAbilitySpecHandle Handle, GameplayAbilityActorInfo ActorInfo, GameplayEffect InGamepayEffect, float GameplayEffectLevel)
        {
            if (InGamepayEffect != null)
            {
                GameplayEffectSpecHandle SpecHandle = MakeOutgoingGameplayEffectSpec(Handle, ActorInfo, InGamepayEffect, GameplayEffectLevel);
            }
        }
        public GameplayEffectSpecHandle MakeOutgoingGameplayEffectSpec(GameplayAbilitySpecHandle Handle, GameplayAbilityActorInfo ActorInfo, GameplayEffect InGamepayEffect, float GameplayEffectLevel)
        {
            AbilitySystemComponent AbilitySystemComponent = ActorInfo.AbilitySystemComponent;
            GameplayEffectSpecHandle NewHandle = AbilitySystemComponent.MakeOutgoingSpec(null, GameplayEffectLevel, null);
            return null;
        }
    }
}

