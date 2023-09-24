using RailShootGame;
using UnityEditor.PackageManager;

namespace GameplayAbilitySystem
{
    public class CooldownGameplayEffect : UGameplayEffect
    {

    }
    public class FGameplayAbilityActorInfo : ReferencePoolObject
    {
        public Actor OwnerActor;
        public Actor AvatarActor;
        public UAbilitySystemComponent AbilitySystemComponent;

    }
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
        public virtual bool CanActivateAbility(FGameplayAbilitySpecHandle Handle)
        {
            return false;
        }
        public void CallActivateAbility(FGameplayAbilitySpecHandle Handle)
        {
            PreActivate(Handle);
            //ActivateAbility(Handle, null);
        }
        public void PreActivate(FGameplayAbilitySpecHandle Handle)
        {
        }
        public virtual void ActivateAbility(FGameplayAbilitySpecHandle Handle, FGameplayAbilityActorInfo ActorInfo, Character owner)
        {
            if (CommitAbility(Handle, ActorInfo))
            {

            }
        }
        public virtual void CancelAbility(FGameplayAbilitySpecHandle Handle)
        {

        }
        public virtual bool CommitAbility(FGameplayAbilitySpecHandle Handle, FGameplayAbilityActorInfo ActorInfo)
        {
            CommitExecute(Handle, ActorInfo);
            return false;
        }
        public void CommitExecute(FGameplayAbilitySpecHandle Handle, FGameplayAbilityActorInfo ActorInfo)
        {
            ApplyCooldown(Handle, ActorInfo);
            ApplyCost();
        }
        public bool CommitAbilityCooldown(FGameplayAbilitySpecHandle Handle, FGameplayAbilityActorInfo ActorInfo)
        {
            ApplyCooldown(Handle, ActorInfo);
            return true;
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

