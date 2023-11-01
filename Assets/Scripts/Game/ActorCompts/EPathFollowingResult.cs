namespace RailShootGame
{
    public enum EPathFollowingResult : int
    {
        /** Reached destination */
        Success,

        /** Movement was blocked */
        Blocked,

        /** Agent is not on path */
        OffPath,

        /** Aborted and stopped (failure) */
        Aborted,

        /** Request was invalid */
        Invalid,
    };
}

