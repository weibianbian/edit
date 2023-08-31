using Core.Timer;
using RailShootGame;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.PackageManager;

namespace GameplayAbilitySystem
{
    public static class GlobalActiveGameplayEffectHandles
    {
        public static Dictionary<FActiveGameplayEffectHandle, AbilitySystemComponent> Map = new Dictionary<FActiveGameplayEffectHandle, AbilitySystemComponent>();
    }
    public class FActiveGameplayEffectHandle
    {
        public int Handle;
        private bool bPassedFiltersAndWasExecuted;
        public FActiveGameplayEffectHandle(int InHandle)
        {
            Handle = InHandle;
            bPassedFiltersAndWasExecuted = true;
        }
        public FActiveGameplayEffectHandle()
        {
            Handle = -1;
            bPassedFiltersAndWasExecuted = false;
        }
        public static FActiveGameplayEffectHandle GenerateNewHandle(AbilitySystemComponent OwningComponent)
        {
            FActiveGameplayEffectHandle NewHandle = new FActiveGameplayEffectHandle();
            GlobalActiveGameplayEffectHandles.Map.Add(NewHandle, OwningComponent);
            return NewHandle;
        }
    }
    public class FActiveGameplayEffect
    {
        public FGameplayEffectSpec Spec;
        public FActiveGameplayEffectHandle Handle;
        public TimerHandle DurationHandle;
        public bool IsPendingRemove;
        public bool bIsInhibited;

        public float GetDuration()
        {
            return Spec.GetDuration();
        }
        //这是将ActiveGE“打开”或“关闭”的核心功能
        public void CheckOngoingTagRequirements(FGameplayTagContainer OwnerTags, FActiveGameplayEffectsContainer OwningContainer, bool bInvokeGameplayCueEvents)
        {
            bool bShouldBeInhibited = !Spec.Def.OngoingTagRequirements.RequirementsMet(OwnerTags);
            if (bIsInhibited != bShouldBeInhibited)
            {
                {
                    // 所有OnDirty回调必须被抑制，直到我们更新整个GameplayEffect。
                    FScopedAggregatorOnDirtyBatch AggregatorOnDirtyBatcher;

                    // 在添加或删除之前设置这个很重要，这样被触发的任何委托都可以准确地查询这个GE。
                    bIsInhibited = bShouldBeInhibited;

                    if (bShouldBeInhibited)
                    {
                        //用属性聚合器移除我们的ActiveGameplayEffects修饰符
                        OwningContainer.RemoveActiveGameplayEffectGrantedTagsAndModifiers(this, bInvokeGameplayCueEvents);
                    }
                    else
                    {
                        OwningContainer.AddActiveGameplayEffectGrantedTagsAndModifiers(this, bInvokeGameplayCueEvents);
                    }
                }
                //EventSet.OnInhibitionChanged.Broadcast(Handle, bIsInhibited);
            }
        }
    }
    public class FGameplayTagRequirements
    {
        public bool RequirementsMet(FGameplayTagContainer Container)
        {

            bool HasRequired = Container.HasAll(RequireTags);
            bool HasIgnored = Container.HasAny(IgnoreTags);

            return HasRequired && !HasIgnored;
        }
    }
}
