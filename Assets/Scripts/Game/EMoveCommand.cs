namespace RailShootGame
{
    public enum EMoveCommand
    {
        MOVE_NONE,
        MOVE_FACE_ENEMY,
        MOVE_FACE_ENTITY,

        // commands < NUM_NONMOVING_COMMANDS don't cause a change in position
        NUM_NONMOVING_COMMANDS,

        MOVE_TO_ENEMY = NUM_NONMOVING_COMMANDS,
        MOVE_TO_ENEMYHEIGHT,
        MOVE_TO_ENTITY,
        MOVE_OUT_OF_RANGE,
        MOVE_TO_ATTACK_POSITION,
        MOVE_TO_COVER,
        MOVE_TO_POSITION,
        MOVE_TO_POSITION_DIRECT,
        MOVE_SLIDE_TO_POSITION,
        MOVE_WANDER,
        NUM_MOVE_COMMANDS
    }
}

