using Core;
using GameplayAbilitySystem;
using System;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

namespace RailShootGame
{
    public class UAbilityTask : UGameplayTask
    {
        public UGameplayAbility Ability;
        public UAbilitySystemComponent AbilitySystemComponent;
        public static T NewAbilityTask<T>(UGameplayAbility ThisAbility, string InstanceName = "") where T : UGameplayTask
        {
            T MyObj = Activator.CreateInstance<T>();
            MyObj.InitTask(ThisAbility, 127);
            MyObj.InstanceName = InstanceName;
            return MyObj;
        }
        public void SetAbilitySystemComponent(UAbilitySystemComponent InAbilitySystemComponent)
        {
            AbilitySystemComponent = InAbilitySystemComponent;
        }
    }
    public class UGameplayTask : IGameplayTaskOwnerInterface
    {
        public EGameplayTaskState TaskState = EGameplayTaskState.Uninitialized;
        public bool bTickingTask = false;
        public bool bOwnedByTasksComponent = true;
        public bool bOwnerFinished = true;
        public int Priority;
        public string InstanceName = "";
        public FGameplayResourceSet RequiredResources;
        public FGameplayResourceSet ClaimedResources;
        public UGameplayTasksComponent TasksComponent;
        IGameplayTaskOwnerInterface TaskOwner;
        public UGameplayTask ChildTask;
        public IGameplayTaskOwnerInterface GetTaskOwner() { return TaskOwner.IsValid() ? TaskOwner : null; }
        public bool IsFinished() { return (TaskState == EGameplayTaskState.Finished); }
        public UGameplayTask GetChildTask() { return ChildTask; }
        public bool IsTickingTask() { return (bTickingTask); }
        public bool IsOwnedByTasksComponent() { return bOwnedByTasksComponent; }
        public bool HasOwnerFinished() { return (bOwnerFinished); }
        public EGameplayTaskState GetState() { return TaskState; }
        public FGameplayResourceSet GetRequiredResources() { return RequiredResources; }
        public FGameplayResourceSet GetClaimedResources() { return ClaimedResources; }
        public virtual void TickTask(float DeltaTime)
        {
            if (TaskState == EGameplayTaskState.Active)
            {
                return;
            }
            TaskState = EGameplayTaskState.Active;
            Activate();
            if (IsFinished() == false)
            {
                TasksComponent.OnGameplayTaskActivated(this);
            }
        }
        public void ActivateInTaskQueue()
        {
            switch (TaskState)
            {
                case EGameplayTaskState.Uninitialized:
                    break;
                case EGameplayTaskState.AwaitingActivation:
                    PerformActivation();
                    break;
                case EGameplayTaskState.Paused:
                    // resume
                    Resume();
                    break;
                case EGameplayTaskState.Active:
                    // nothing to do here
                    break;
                case EGameplayTaskState.Finished:
                    // If a task has finished, and it's being revived let's just treat the same as AwaitingActivation
                    PerformActivation();
                    break;
                default:
                    break;
            }
        }
        public virtual void Activate()
        {

        }
        public virtual bool IsValid()
        {
            return true;
        }

        public void OnGameplayTaskActivated(UGameplayTask Task)
        {

        }
        public void ReadyForActivation()
        {
            if (TasksComponent != null)
            {
                //if (RequiresPriorityOrResourceManagement() == false)
                //{
                //    PerformActivation();
                //}
                //else
                {
                    TasksComponent.AddTaskReadyForActivation(this);
                }
            }
            else
            {
                EndTask();
            }
        }
        public void PerformActivation()
        {
            if (TaskState == EGameplayTaskState.Active)
            {
                return;
            }

            TaskState = EGameplayTaskState.Active;

            Activate();

            // Activate call may result in the task actually "instantly" finishing.
            // If this happens we don't want to bother the TaskComponent
            // with information on this task
            if (IsFinished() == false)
            {
                TasksComponent.OnGameplayTaskActivated(this);
            }
        }
        public void Pause()
        {
            TaskState = EGameplayTaskState.Paused;

            TasksComponent.OnGameplayTaskDeactivated(this);
        }

        public void Resume()
        {
            TaskState = EGameplayTaskState.Active;
            TasksComponent.OnGameplayTaskActivated(this);
        }

        public void OnGameplayTaskDeactivated(UGameplayTask Task)
        {
        }
        public void TaskOwnerEnded()
        {
            if (TaskState != EGameplayTaskState.Finished)
            {
                bOwnerFinished = true;
                OnDestroy(true);
            }
        }
        public void EndTask()
        {
            if (TaskState != EGameplayTaskState.Finished)
            {
                OnDestroy(false);
            }
        }
        public virtual void OnDestroy(bool bInOwnerFinished)
        {
            TaskState = EGameplayTaskState.Finished;
            TasksComponent.OnGameplayTaskDeactivated(this);
        }
        public void InitTask(IGameplayTaskOwnerInterface InTaskOwner, int InPriority)
        {
            Priority = InPriority;
            TaskOwner = InTaskOwner;
            TaskState = EGameplayTaskState.AwaitingActivation;

            InTaskOwner.OnGameplayTaskInitialized(this);
            UGameplayTasksComponent GTComponent = InTaskOwner.GetGameplayTasksComponent(this);
            TasksComponent = GTComponent;
            //bOwnedByTasksComponent = (TaskOwner.GetObject() == GTComponent);
        }

        public void OnGameplayTaskInitialized(UGameplayTask Task)
        {
            throw new System.NotImplementedException();
        }

        public UGameplayTasksComponent GetGameplayTasksComponent(UGameplayTask Task)
        {
            throw new System.NotImplementedException();
        }
    }
}

