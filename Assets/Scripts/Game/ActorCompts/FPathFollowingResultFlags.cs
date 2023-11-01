namespace RailShootGame
{
    public enum FPathFollowingResultFlags
    {
        None = 0,
        /** Reached destination (EPathFollowingResult::Success) */
        Success = (1 << 0),
        /** Movement was blocked (EPathFollowingResult::Blocked) */
        Blocked = (1 << 1),
        /** Agent is not on path (EPathFollowingResult::OffPath) */
        OffPath = (1 << 2),
        /** Aborted (EPathFollowingResult::Aborted) */
        UserAbort = (1 << 3),
        /** Abort details: owner no longer wants to move */
        OwnerFinished = (1 << 4),
        /** Abort details: path is no longer valid */
        InvalidPath = (1 << 5),
        /** Abort details: unable to move */
        MovementStop = (1 << 6),
        /** Abort details: new movement request was received */
        NewRequest = (1 << 7),
        /** Abort details: blueprint MoveTo function was called */
        ForcedScript = (1 << 8),
        /** Finish details: never started, agent was already at goal */
        AlreadyAtGoal = (1 << 9),
        /** Can be used to create project specific reasons */
        FirstGameplayFlagShift = 10,
        UserAbortFlagMask = ~(Success | Blocked | OffPath),
    }
}

