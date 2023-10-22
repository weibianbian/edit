using UnityEngine;
public class playerPState_s
{
    public Vector3 origin;
    public Vector3 velocity;
    public Vector3 localOrigin;
    public Vector3 pushVelocity;
    public float stepUp;
    public int movementType;
    public int movementFlags;
    public int movementTime;
}
public class idPhysics_Player : MonoBehaviour
{
    bool walking = false;
    bool groundPlane = false;
    bool ladder = false;
    int framemsec = 0;
    float frametime = 0;
    float walkSpeed = 4;
    float playerSpeed = 0;
    public playerPState_s current = new playerPState_s();
    public Vector3 inputDir = Vector3.zero;
    float maxJumpHeight = 10;
    Vector3 gravityVector;
    public int upmove = 0;
    const int PMF_DUCKED = 1;       // set when ducking
    const int PMF_JUMPED = 2;       // set when the player jumped this frame
    const int PMF_STEPPED_UP = 4;       // set when the player stepped up this frame
    const int PMF_STEPPED_DOWN = 8;     // set when the player stepped down this frame
    const int PMF_JUMP_HELD = 16;       // set when jump button is held down
    const int PMF_TIME_LAND = 32;       // movementTime is time before rejump
    const int PMF_TIME_KNOCKBACK = 64;      // movementTime is an air-accelerate only time
    const int PMF_TIME_WATERJUMP = 128;     // movementTime is waterjump
    const int PMF_ALL_TIMES = (PMF_TIME_WATERJUMP | PMF_TIME_LAND | PMF_TIME_KNOCKBACK);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        inputDir = new Vector3(horizontal, 0, vertical);
        inputDir = Camera.main.transform.TransformVector(inputDir);

        current.origin = transform.position;
        MovePlayer((int)(Time.deltaTime * 1000));


        transform.position = current.origin;
        //transform.position += (Time.deltaTime * inputDir * 10);
    }
    public void MovePlayer(int msec)
    {
        framemsec = msec;
        frametime = framemsec * 0.001f;

        playerSpeed = walkSpeed;

        CheckGround();
        WalkMove();
    }
    public void WalkMove()
    {
        if (CheckJump())
        {
            return;
        }

        Friction();
        Vector3 wishvel;
        Vector3 wishdir;
        float wishspeed;
        float scale;
        float accelerate = 10;
        Vector3 oldVelocity, vel;
        float oldVel, newVel;

        wishvel = inputDir;

        wishdir = wishvel;

        wishspeed = playerSpeed;


        wishdir.y = 0;
        wishdir.Normalize();
        Accelerate(wishdir, wishspeed, accelerate);

        oldVelocity = current.velocity;

        SlideMove(false, true, true, true);
    }
    void SlideMove(bool gravity, bool stepUp, bool stepDown, bool push)
    {
        Vector3 end, stepEnd, primal_velocity, endVelocity, endClipVelocity, clipVelocity;
        float time_left;
        {
            endVelocity = current.velocity;
        }
        time_left = frametime;
        end = current.origin + time_left * current.velocity;
        current.origin = end;


    }
    void Accelerate(Vector3 wishdir, float wishspeed, float accel)
    {
        // q2 style
        float addspeed, accelspeed, currentspeed;

        currentspeed = Vector3.Dot(current.velocity, wishdir);
        addspeed = wishspeed - currentspeed;
        if (addspeed <= 0)
        {
            return;
        }
        accelspeed = accel * frametime * wishspeed;
        if (accelspeed > addspeed)
        {
            accelspeed = addspeed;
        }
        current.velocity += (accelspeed * wishdir);
    }
    public void Friction()
    {
        Vector3 vel;
        float speed, newspeed, control;
        float drop;

        vel = current.velocity;

        if (walking)
        {
            // ignore slope movement, remove all velocity in gravity direction
        }
        speed = vel.sqrMagnitude;
        if (speed <= 0.0f)
        {
            current.velocity = Vector3.zero;
            return;
        }
        drop = 0;
        {
            {
                control = speed;
                drop += control * 6 * frametime;
            }
        }
        newspeed = speed - drop;
        if (newspeed < 0)
        {
            newspeed = 0;
        }
        current.velocity *= (newspeed / speed);
    }
    public void CheckGround()
    {
        RaycastHit hit;
        Ray ray = new Ray(current.origin, Vector3.down);
        Physics.Raycast(ray, out hit, 100);
        {
            float dist = Vector3.Distance(hit.point, current.origin);
            if (dist != 2)
            {
                current.origin.y = hit.point.y + 2;
            }
        }
    }
    public bool CheckJump()
    {
        Vector3 addVelocity;

        if (upmove < 10)
        {
            return false;
        }

        if ((current.movementFlags & PMF_JUMP_HELD) == 0)
        {
            return false;
        }

        groundPlane = false;
        walking = false;
        current.movementFlags |= PMF_JUMP_HELD | PMF_JUMPED;

        addVelocity = 2.0f * maxJumpHeight * -gravityVector;
        current.velocity = addVelocity;
        return true;
    }
}
