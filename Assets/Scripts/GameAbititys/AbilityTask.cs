using RailShootGame;
using System;
using UnityEngine;

namespace GameplayAbilitySystem
{
    public class AbilityTaskWaitOverlap : UAbilityTask
    {
        public static AbilityTaskWaitOverlap WaitForOverlap(UGameplayAbility OwningAbility)
        {
            AbilityTaskWaitOverlap MyObj = NewAbilityTask<AbilityTaskWaitOverlap>(OwningAbility,"");
            return MyObj;
        }
        public void OnHitCallback(AActor HitActor, AActor OtherActor, Vector3 NormalImpulse)
        {
            if (OtherActor != null)
            {
                // Construct TargetData
                //FGameplayAbilityTargetData_SingleTargetHit* TargetData = new FGameplayAbilityTargetData_SingleTargetHit(Hit);

                //// Give it a handle and return
                //FGameplayAbilityTargetDataHandle Handle;
                //Handle.Data.Add(TSharedPtr<FGameplayAbilityTargetData>(TargetData));
                //if (ShouldBroadcastAbilityTaskDelegates())
                //{
                //    OnOverlap.Broadcast(Handle);
                //}

                // We are done. Kill us so we don't keep getting broadcast messages
                EndTask();
            }
        }
        public override void Activate()
        {

        }
    }
}

