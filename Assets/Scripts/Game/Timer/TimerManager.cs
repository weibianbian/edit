using System.Collections.Generic;

namespace RailShootGame
{
    public class TimerManager
    {
        List<TimerHandle> ActiveTimerHeap = new List<TimerHandle>();
        List<TimerData> Timers = new List<TimerData>();
        List<TimerHandle> PendingTimerSet = new List<TimerHandle>();
        List<TimerHandle> PausedTimerSet = new List<TimerHandle>();
        TimerHandle CurrentlyExecutingTimer;
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
                TimerHandle TopHandle = ActiveTimerHeap[0];
                int TopIndex = TopHandle.GetIndex();
                TimerData Top = Timers[TopIndex];
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
                        }
                        else
                        {
                            RemoveTimer(CurrentlyExecutingTimer);
                        }
                        CurrentlyExecutingTimer.Invalidate();
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
                    TimerHandle Handle = PendingTimerSet[i];
                    TimerData TimerToActivate = GetTimer(Handle);
                    TimerToActivate.ExpireTime += InternalTime;
                    TimerToActivate.Status = ETimerStatus.Active;
                    ActiveTimerHeap.Add(Handle);
                }
                PendingTimerSet.Clear();
            }
        }
        public TimerHandle AddTimer(TimerData TimerData)
        {
            Timers.Add(TimerData);
            TimerHandle Result = GenerateHandle(Timers.Count - 1);
            Timers[Timers.Count - 1].Handle = Result;
            return Result;
        }

        public void RemoveTimer(TimerHandle Handle)
        {
            Timers.RemoveAt(Handle.GetIndex());
        }
        public TimerData GetTimer(TimerHandle InHandle)
        {
            int Index = InHandle.GetIndex();
            TimerData Timer = Timers[Index];
            return Timer;
        }
        TimerHandle GenerateHandle(int Index)
        {
            ulong NewSerialNumber = ++LastAssignedSerialNumber;
            if (!(NewSerialNumber != TimerHandle.MaxSerialNumber))
            {
                NewSerialNumber = (ulong)1;
            }

            TimerHandle Result = new TimerHandle();
            Result.SetIndexAndSerialNumber(Index, NewSerialNumber);
            return Result;
        }
        public void SetTimer(ref TimerHandle InOutHandle, ITimerDelegate InDelegate, float InRate, bool InbLoop, float InFirstDelay = -1.0f)
        {
            InternalSetTimer(ref InOutHandle, InDelegate, InRate, InbLoop, InFirstDelay);
        }
        bool HasBeenTickedThisFrame()
        {
            //return (LastTickedFrame == GFrameCounter);
            return false;
        }
        public TimerData FindTimer(TimerHandle InHandle)
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
            TimerData Timer = Timers[Index];
            if (Timer.Handle != InHandle || Timer.Status == ETimerStatus.ActivePendingRemoval)
            {
                return null;
            }

            return Timer;
        }
        public void InternalClearTimer(TimerHandle InHandle)
        {
            TimerData Data = GetTimer(InHandle);
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
        public void InternalSetTimer(ref TimerHandle InOutHandle, ITimerDelegate InDelegate, float InRate, bool InbLoop, float InFirstDelay)
        {
            if (FindTimer(InOutHandle) != null)
            {
                // if the timer is already set, just clear it and we'll re-add it, since 
                // there's no data to maintain.
                InternalClearTimer(InOutHandle);
            }
            if (InRate > 0.0f)
            {
                TimerData NewTimerData = new TimerData()
                {
                    Rate = InRate,
                    bLoop = InbLoop,
                    TimerDelegate = InDelegate,
                };
                float FirstDelay = (InFirstDelay >= 0.0f) ? InFirstDelay : InRate;

                TimerHandle NewTimerHandle = new TimerHandle();

                if (HasBeenTickedThisFrame())
                {
                    NewTimerData.ExpireTime = InternalTime + FirstDelay;
                    NewTimerData.Status = ETimerStatus.Active;
                    NewTimerHandle = AddTimer(NewTimerData);
                    ActiveTimerHeap.Add(NewTimerHandle);
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

