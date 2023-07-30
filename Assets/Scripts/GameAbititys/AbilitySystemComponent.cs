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
        public static bool operator ==(GameplayAbilitySpecHandle a, GameplayAbilitySpecHandle b)
        {
            return a.Handle == b.Handle;
        }
        public static bool operator !=(GameplayAbilitySpecHandle a, GameplayAbilitySpecHandle b)
        {
            return a.Handle != b.Handle;
        }

    }

    public class AbilitySystemComponent : ActorCompt
    {
        public GameplayAbilitySpecContainer ActivatableAbilities;
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
            return null;
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

    }
}

