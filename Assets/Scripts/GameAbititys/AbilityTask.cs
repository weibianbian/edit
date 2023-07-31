using RailShootGame;
using System;
using UnityEngine;

namespace GameplayAbilitySystem
{
    public class AbilityTaskWaitOverlap : AbilityTask
    {
        public static AbilityTaskWaitOverlap WaitForOverlap(GameplayAbility OwningAbility)
        {
            AbilityTaskWaitOverlap MyObj = NewAbilityTask<AbilityTaskWaitOverlap>(OwningAbility);
            return MyObj;
        }
        public void OnHitCallback(Actor HitActor, Actor OtherActor, Vector3 NormalImpulse)
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
    public class AbilityTask
    {
        public EGameplayTaskState TaskState;
        public static T NewAbilityTask<T>(GameplayAbility ThisAbility) where T : AbilityTask
        {
            T MyObj = Activator.CreateInstance<T>();
            MyObj.InitTask(ThisAbility);
            return MyObj;
        }
        public void InitTask(GameplayAbility InTaskOwner)
        {

        }
        public void EndTask()
        {
            if (TaskState != EGameplayTaskState.Finished)
            {
                OnDestroy();
            }
        }
        public void OnDestroy()
        {
            TaskState = EGameplayTaskState.Finished;
        }
        public virtual void Activate()
        {

        }
    }
}

