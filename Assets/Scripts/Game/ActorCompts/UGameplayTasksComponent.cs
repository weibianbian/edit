using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using UnityEngine;

namespace RailShootGame
{
    public enum EGameplayTaskEvent
    {
        Add,
        Remove,
    }
    public class FGameplayTaskEventData
    {
        public EGameplayTaskEvent Event;
        public UGameplayTask RelatedTask;

        public FGameplayTaskEventData(EGameplayTaskEvent InEvent, UGameplayTask InRelatedTask)
        {
            Event = InEvent;
            RelatedTask = InRelatedTask;
        }
    }
    public class FGameplayResourceSet
    {

    }
    public class UGameplayTasksComponent : ActorComponent
    {
        public List<FGameplayTaskEventData> TaskEvents = new List<FGameplayTaskEventData>();
        public List<UGameplayTask> TickingTasks = new List<UGameplayTask>();
        public List<UGameplayTask> TaskPriorityQueue = new List<UGameplayTask>();
        public override void TickComponent(float DeltaTime)
        {
            base.TickComponent(DeltaTime);
            int NumTickingTasks = TickingTasks.Count();
            int NumActuallyTicked = 0;
            switch (NumActuallyTicked)
            {
                case 0:
                    break;
                case 1:
                    {
                        UGameplayTask TickingTask = TickingTasks[0];
                        TickingTask.TickTask(DeltaTime);
                        NumActuallyTicked++;
                    }
                    break;
                default:
                    {
                        foreach (UGameplayTask TickingTask in TickingTasks)
                        {
                            {
                                TickingTask.TickTask(DeltaTime);
                                NumActuallyTicked++;
                            }
                        }
                    }
                    break;
            }
        }
        public void UpdateTaskActivations()
        {
            if (TaskPriorityQueue.Count > 0)
            {
                List<UGameplayTask> ActivationList = new List<UGameplayTask>();
                for (int TaskIndex = 0; TaskIndex < TaskPriorityQueue.Count; ++TaskIndex)
                {
                    if (TaskPriorityQueue[TaskIndex] != null)
                    {
                        FGameplayResourceSet RequiredResources = TaskPriorityQueue[TaskIndex].GetRequiredResources();
                        FGameplayResourceSet ClaimedResources = TaskPriorityQueue[TaskIndex].GetClaimedResources();
                    }
                }
                for (int Idx = 0; Idx < ActivationList.Count; Idx++)
                {
                    // check if task wasn't already finished as a result of activating previous elements of this list
                    if (ActivationList[Idx].IsFinished() == false)
                    {
                        ActivationList[Idx].ActivateInTaskQueue();
                    }
                }
            }
        }
        public void AddTaskReadyForActivation(UGameplayTask NewTask)
        {
            TaskEvents.Add(new FGameplayTaskEventData(EGameplayTaskEvent.Add, NewTask));
            // trigger the actual processing only if it was the first event added to the list
            if (TaskEvents.Count == 1)
            {
                ProcessTaskEvents();
            }
        }
        public void ProcessTaskEvents()
        {
            UpdateTaskActivations();
        }
        public virtual void OnGameplayTaskActivated(UGameplayTask Task)
        {
            if (Task.IsTickingTask())
            {
                TickingTasks.Add(Task);
                if (TickingTasks.Count() == 1)
                {

                }
            }
            IGameplayTaskOwnerInterface TaskOwner = Task.GetTaskOwner();
            if (!Task.IsOwnedByTasksComponent() && TaskOwner != null)
            {
                TaskOwner.OnGameplayTaskActivated(Task);
            }
        }

        public virtual void OnGameplayTaskDeactivated(UGameplayTask Task)
        {
            bool bIsFinished = (Task.GetState() == EGameplayTaskState.Finished);

            if (Task.GetChildTask() != null && bIsFinished)
            {
                if (Task.HasOwnerFinished())
                {
                    Task.GetChildTask().TaskOwnerEnded();
                }
                else
                {
                    Task.GetChildTask().EndTask();
                }
            }

            if (Task.IsTickingTask())
            {
                // 如果我们要删除最后一个计时任务，请将此组件设置为非活动状态，以便它停止计时
                TickingTasks.Remove(Task);
            }

            if (bIsFinished)
            {
                //使用RemoveSwap而不是RemoveSingleSwap，因为可以添加任务
                //激活和取消暂停时的KnownTasks
                //删除只发生一次。在这里处理比较便宜。
                //KnownTasks.RemoveSwap(&Task);
            }
            // Resource-using task
            if (bIsFinished)
            {
                OnTaskEnded(Task);
            }

            IGameplayTaskOwnerInterface TaskOwner = Task.GetTaskOwner();
            if (!Task.IsOwnedByTasksComponent() && !Task.HasOwnerFinished() && TaskOwner != null)
            {
                TaskOwner.OnGameplayTaskDeactivated(Task);
            }
        }
        public void OnTaskEnded(UGameplayTask Task)
        {
            RemoveResourceConsumingTask(Task);
        }
        public void RemoveResourceConsumingTask(UGameplayTask Task)
        {
            //TaskEvents.Add(FGameplayTaskEventData(EGameplayTaskEvent.Remove, Task));
            // trigger the actual processing only if it was the first event added to the list
            //if (TaskEvents.Num() == 1 && CanProcessEvents())
            //{
            //    ProcessTaskEvents();
            //}
        }
        public virtual UGameplayTasksComponent GetGameplayTasksComponent(UGameplayTask Task) { return this; }
    }
}

