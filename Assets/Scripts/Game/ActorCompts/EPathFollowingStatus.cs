namespace RailShootGame
{
    public enum EPathFollowingStatus
    {
        Idle,

        /** Request with incomplete path, will start after UpdateMove() */
        Waiting,

        /** Request paused, will continue after ResumeMove() */
        Paused,

        /** Following path */
        Moving,
    }
}

