using Sirenix.OdinInspector;
using UnityEngine;
public enum EStatus
{
    None,
    Patrol,
    Combat,
    Alert,
}
//public enum ENodeResult
//{
//    Succeeded=0,
//    Failed=1,
//    InProgress=2,
//}
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
public enum EMoveStatus
{
    MOVE_STATUS_DONE,
    MOVE_STATUS_MOVING,
    MOVE_STATUS_WAITING,
    MOVE_STATUS_DEST_NOT_FOUND,
    MOVE_STATUS_DEST_UNREACHABLE,
    MOVE_STATUS_BLOCKED_BY_WALL,
    MOVE_STATUS_BLOCKED_BY_OBJECT,
    MOVE_STATUS_BLOCKED_BY_ENEMY,
    MOVE_STATUS_BLOCKED_BY_MONSTER
}
public enum EMoveType
{
    MOVETYPE_DEAD,
    MOVETYPE_ANIM,
    MOVETYPE_SLIDE,
    MOVETYPE_FLY,
    MOVETYPE_STATIC,
    NUM_MOVETYPES
}
public class MoveState
{
    public EMoveType moveType;
    public EMoveCommand moveCommand;
    public EMoveStatus moveStatus;
}
public static class Vector3Extern
{
    public static float ToYaw(this Vector3 vec)
    {
        float yaw;

        if ((vec.y == 0.0f) && (vec.x == 0.0f))
        {
            yaw = 0.0f;
        }
        else
        {
            //yaw = RAD2DEG(Mathf.Atan2( vec.y, vec.x));
            //if (yaw < 0.0f)
            //{
            //    yaw += 360.0f;
            //}
        }
        //return yaw;
        return 0;
    }
}
public class Actor
{
    public EStatus state;
    public Vector3 origin;
    public float ideal_yaw;
    bool TurnToward(Vector3 pos)
    {
        Vector3 dir;
        Vector3 local_dir;
        float lengthSqr;

        dir = pos - origin;
        local_dir = Vector3.ProjectOnPlane(dir, new Vector3(0, 1, 0));
        lengthSqr = local_dir.sqrMagnitude;
        if (lengthSqr > Mathf.Pow(2.0f, 2) || (lengthSqr > Mathf.Pow(0.1f, 2)))
        {
            //ideal_yaw = AngleNormalize180(local_dir.ToYaw());
        }

        //bool result = FacingIdeal();
        //return result;
        return true;
    }
}
