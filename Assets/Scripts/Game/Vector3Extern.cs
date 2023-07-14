using UnityEngine;

namespace RailShootGame
{
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
}

