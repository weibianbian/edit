using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine.Tilemaps;

namespace Core.Timer
{
    public struct FTimerHeapOrder : IComparer<FTimerHandle>
    {
        List<FTimerData> Timers;

        public FTimerHeapOrder(List<FTimerData> InTimers)
        {
            Timers = InTimers;
        }

        public int Compare(FTimerHandle LhsHandle, FTimerHandle RhsHandle)
        {
            int LhsIndex = LhsHandle.GetIndex();
            int RhsIndex = RhsHandle.GetIndex();

            FTimerData LhsData = Timers[LhsIndex];
            FTimerData RhsData = Timers[RhsIndex];

            return (LhsData.ExpireTime > RhsData.ExpireTime) ? 1 : -1;
        }
    }
    public class FTimerManager
    {
        List<FTimerHandle> ActiveTimerHeap = new List<FTimerHandle>();
        List<FTimerData> Timers = new List<FTimerData>();
        List<FTimerHandle> PendingTimerSet = new List<FTimerHandle>();
        List<FTimerHandle> PausedTimerSet = new List<FTimerHandle>();
        FTimerHandle CurrentlyExecutingTimer;
        double InternalTime;
        ulong LastAssignedSerialNumber = 0;
        ulong LastTickedFrame;
        public void Tick(float DeltaTime)
        {
            if (HasBeenTickedThisFrame())
            {
                return;
            }
            InternalTime += DeltaTime;
            while (ActiveTimerHeap.Count > 0)
            {
                FTimerHandle TopHandle = ActiveTimerHeap[0];
                int TopIndex = TopHandle.GetIndex();
                FTimerData Top = Timers[TopIndex];
                if (Top.Status == ETimerStatus.ActivePendingRemoval)
                {
                    ActiveTimerHeap.RemoveAt(0);
                    RemoveTimer(TopHandle);
                    continue;
                }
                if (InternalTime > Top.ExpireTime)
                {
                    CurrentlyExecutingTimer = ActiveTimerHeap[0];
                    ActiveTimerHeap.RemoveAt(0);
                    Top.Status = ETimerStatus.Executing;
                    int CallCount = Top.bLoop ? (int)(((InternalTime - Top.ExpireTime) / Top.Rate) + 1) : 1;
                    for (int CallIdx = 0; CallIdx < CallCount; ++CallIdx)
                    {
                        Top.TimerDelegate?.Execute();
                        Top = FindTimer(CurrentlyExecutingTimer);
                        if (Top == null || Top.Status != ETimerStatus.Executing)
                        {
                            break;
                        }
                    }
                    if (Top != null)
                    {
                        if (Top.bLoop)
                        {
                            Top.ExpireTime += CallCount * Top.Rate;
                            Top.Status = ETimerStatus.Active;
                            ActiveTimerHeap.Add(CurrentlyExecutingTimer);
                            ActiveTimerHeap.Sort(new FTimerHeapOrder(Timers));
                        }
                        else
                        {
                            RemoveTimer(CurrentlyExecutingTimer);
                            //CurrentlyExecutingTimer.Invalidate();
                        }

                    }
                }
                else
                {
                    break;
                }
            }
            if (PendingTimerSet.Count > 0)
            {
                for (int i = 0; i < PendingTimerSet.Count; i++)
                {
                    FTimerHandle Handle = PendingTimerSet[i];
                    FTimerData TimerToActivate = GetTimer(Handle);
                    TimerToActivate.ExpireTime += InternalTime;
                    TimerToActivate.Status = ETimerStatus.Active;
                    ActiveTimerHeap.Add(Handle);
                }
                ActiveTimerHeap.Sort(new FTimerHeapOrder(Timers));
                PendingTimerSet.Clear();
            }
        }
        public FTimerHandle AddTimer(FTimerData TimerData)
        {
            Timers.Add(TimerData);
            FTimerHandle Result = GenerateHandle(Timers.Count - 1);
            Timers[Timers.Count - 1].Handle = Result;
            return Result;
        }

        public void RemoveTimer(FTimerHandle Handle)
        {
            Timers.RemoveAt(Handle.GetIndex());
        }
        public FTimerData GetTimer(FTimerHandle InHandle)
        {
            int Index = InHandle.GetIndex();
            FTimerData Timer = Timers[Index];
            return Timer;
        }
        FTimerHandle GenerateHandle(int Index)
        {
            ulong NewSerialNumber = ++LastAssignedSerialNumber;
            if (!(NewSerialNumber != FTimerHandle.MaxSerialNumber))
            {
                NewSerialNumber = (ulong)1;
            }

            FTimerHandle Result = new FTimerHandle();
            Result.SetIndexAndSerialNumber(Index, NewSerialNumber);
            return Result;
        }
        public void SetTimer(ref FTimerHandle InOutHandle, ITimerDelegate InDelegate, float InRate, bool InbLoop, float InFirstDelay = -1.0f)
        {
            InternalSetTimer(ref InOutHandle, InDelegate, InRate, InbLoop, InFirstDelay);
        }
        bool HasBeenTickedThisFrame()
        {
            //return (LastTickedFrame == GFrameCounter);
            return false;
        }
        public FTimerData FindTimer(FTimerHandle InHandle)
        {
            if (!InHandle.IsValid())
            {
                return null;
            }
            int Index = InHandle.GetIndex();
            if (Index < 0 || Index >= Timers.Count)
            {
                return null;
            }
            FTimerData Timer = Timers[Index];
            if (Timer.Handle != InHandle || Timer.Status == ETimerStatus.ActivePendingRemoval)
            {
                return null;
            }

            return Timer;
        }
        public void InternalClearTimer(FTimerHandle InHandle)
        {
            FTimerData Data = GetTimer(InHandle);
            switch (Data.Status)
            {
                case ETimerStatus.Pending:
                    {
                        PendingTimerSet.Remove(InHandle);
                        RemoveTimer(InHandle);
                    }
                    break;

                case ETimerStatus.Active:
                    Data.Status = ETimerStatus.ActivePendingRemoval;
                    break;

                case ETimerStatus.ActivePendingRemoval:
                    // Already removed
                    break;

                case ETimerStatus.Paused:
                    {
                        PausedTimerSet.Remove(InHandle);
                        RemoveTimer(InHandle);
                    }
                    break;

                case ETimerStatus.Executing:
                    // Edge case. We're currently handling this timer when it got cleared.  Clear it to prevent it firing again
                    // in case it was scheduled to fire multiple times.
                    CurrentlyExecutingTimer.Invalidate();
                    RemoveTimer(InHandle);
                    break;
                default:
                    break;
            }
        }
        public void ClearTimer(FTimerHandle InHandle)
        {
            FTimerData TimerData = FindTimer(InHandle);
            if (TimerData != null)
            {
                InternalClearTimer(InHandle);
            }
            InHandle.Invalidate();
        }
        public bool TimerExists(FTimerHandle InHandle)
        {
            return FindTimer(InHandle) != null;
        }
        public FTimerHandle SetTimerForNextTick(ITimerDelegate InDelegate)
        {
            return InternalSetTimerForNextTick((InDelegate));
        }
        public FTimerHandle InternalSetTimerForNextTick(ITimerDelegate InDelegate)
        {
            FTimerData NewTimerData = new FTimerData();
            NewTimerData.Rate = 0.0f;
            NewTimerData.bLoop = false;
            NewTimerData.bRequiresDelegate = true;
            NewTimerData.TimerDelegate = (InDelegate);
            NewTimerData.ExpireTime = InternalTime;
            NewTimerData.Status = ETimerStatus.Active;
            FTimerHandle NewTimerHandle = AddTimer((NewTimerData));
            ActiveTimerHeap.Add(NewTimerHandle);
            ActiveTimerHeap.Sort(new FTimerHeapOrder(Timers));
            return NewTimerHandle;
        }
        public float GetTimerRemaining(FTimerHandle InHandle)
        {

            FTimerData TimerData = FindTimer(InHandle);
            return InternalGetTimerRemaining(TimerData);
        }
        public float InternalGetTimerRemaining(FTimerData TimerData)
        {
            if (TimerData != null)
            {
                switch (TimerData.Status)
                {
                    case ETimerStatus.Active:
                        return (float)(TimerData.ExpireTime - InternalTime);

                    case ETimerStatus.Executing:
                        return 0.0f;

                    default:
                        //过期时间是暂停计时器的剩余时间
                        return (float)TimerData.ExpireTime;
                }
            }

            return -1.0f;
        }
        public void InternalSetTimer(ref FTimerHandle InOutHandle, ITimerDelegate InDelegate, float InRate, bool InbLoop, float InFirstDelay)
        {
            if (FindTimer(InOutHandle) != null)
            {
                // if the timer is already set, just clear it and we'll re-add it, since 
                // there's no data to maintain.
                InternalClearTimer(InOutHandle);
            }
            if (InRate > 0.0f)
            {
                FTimerData NewTimerData = new FTimerData()
                {
                    Rate = InRate,
                    bLoop = InbLoop,
                    TimerDelegate = InDelegate,
                };
                float FirstDelay = (InFirstDelay >= 0.0f) ? InFirstDelay : InRate;

                FTimerHandle NewTimerHandle = new FTimerHandle();

                if (HasBeenTickedThisFrame())
                {
                    NewTimerData.ExpireTime = InternalTime + FirstDelay;
                    NewTimerData.Status = ETimerStatus.Active;
                    NewTimerHandle = AddTimer(NewTimerData);
                    ActiveTimerHeap.Add(NewTimerHandle);
                    ActiveTimerHeap.Sort(new FTimerHeapOrder(Timers));
                }
                else
                {
                    // Store time remaining in ExpireTime while pending
                    NewTimerData.ExpireTime = FirstDelay;
                    NewTimerData.Status = ETimerStatus.Pending;
                    NewTimerHandle = AddTimer(NewTimerData);
                    PendingTimerSet.Add(NewTimerHandle);
                }

                InOutHandle = NewTimerHandle;
            }
            else
            {
                InOutHandle.Invalidate();
            }

        }
    }
}

