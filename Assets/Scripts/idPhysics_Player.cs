using System.Diagnostics.Contracts;
using Unity.VisualScripting;
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
public enum pmtype_t
{
    PM_NORMAL,              // normal physics
    PM_DEAD,                // no acceleration or turning, but free falling
    PM_SPECTATOR,           // flying without gravity but with collision detection
    PM_FREEZE,              // stuck in place without control
    PM_NOCLIP				// flying without collision detection nor gravity
}
public class idPhysics_Player : MonoBehaviour
{
    public bool walking = false;
    public bool groundPlane = false;
    public bool contacts = false;
    public bool ladder = false;
    int framemsec = 0;
    float frametime = 0;
    public float walkSpeed = 100;
    public float playerSpeed = 0;
    public playerPState_s current = new playerPState_s();
    public Vector3 inputDir = Vector3.zero;
    float maxJumpHeight = 10;
    Vector3 gravityVector = new Vector3(0, -10, 0);
    public int upmove = 0;
    //
    const float PM_STOPSPEED = 100.0f;
    const float PM_SWIMSCALE = 0.5f;
    const float PM_LADDERSPEED = 100.0f;
    const float PM_STEPSCALE = 1.0f;

    const float PM_ACCELERATE = 10.0f;
    const float PM_AIRACCELERATE = 1.0f;
    const float PM_WATERACCELERATE = 4.0f;
    const float PM_FLYACCELERATE = 8.0f;

    const float PM_FRICTION = 6.0f;
    const float PM_AIRFRICTION = 0.0f;
    const float PM_WATERFRICTION = 1.0f;
    const float PM_FLYFRICTION = 3.0f;
    const float PM_NOCLIPFRICTION = 12.0f;

    const float MIN_WALK_NORMAL = 0.7f;     // can't walk on very steep slopes
    const float OVERCLIP = 1.001f;
    //

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
        inputDir = new Vector3();
        upmove = 0;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        inputDir = new Vector3(horizontal, 0, vertical);
        inputDir = Camera.main.transform.TransformVector(inputDir);
        inputDir.y = 0;
        inputDir.Normalize();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            upmove = 1;
        }

    }
    private void FixedUpdate()
    {
        current.origin = transform.position;
        MovePlayer((int)(Time.deltaTime * 1000));
        current.origin = current.origin + Time.deltaTime * current.velocity; ;
        transform.position = current.origin;
    }
    public void MovePlayer(int msec)
    {
        walking = false;
        groundPlane = false;
        ladder = false;

        framemsec = msec;
        frametime = framemsec * 0.001f;

        playerSpeed = walkSpeed;

        current.movementFlags &= ~(PMF_JUMPED | PMF_STEPPED_UP | PMF_STEPPED_DOWN);
        current.stepUp = 0.0f;

        if (upmove < 1)
        {
            // not holding jump
            current.movementFlags &= ~PMF_JUMP_HELD;
        }
        if (current.movementType == (int)pmtype_t.PM_FREEZE)
        {
            return;
        }
        if (current.movementType == (int)pmtype_t.PM_DEAD)
        {
            inputDir = Vector3.zero;
            upmove = 0;
        }
        CheckGround();
        if (walking)
        {
            WalkMove();
        }
        else
        {
            AirMove();
        }
        CheckGround();
    }
    public void WalkMove()
    {
        Vector3 wishvel;
        Vector3 wishdir;
        float wishspeed;
        float scale;
        float accelerate;
        Vector3 oldVelocity, vel;
        float oldVel, newVel;
        if (CheckJump())
        {
            AirMove();
            return;
        }
        Friction();
        wishvel = inputDir;
        wishdir = wishvel;
        wishspeed = playerSpeed;
        wishdir.y = 0;
        wishdir.Normalize();

        accelerate = PM_ACCELERATE;
        Accelerate(wishdir, wishspeed, accelerate);
        oldVelocity = current.velocity;

        current.velocity.y = 0;

        vel = current.velocity - Vector3.Dot(current.velocity, gravityVector.normalized) * gravityVector.normalized;
        if (!(vel.sqrMagnitude != 0))
        {
            return;
        }

        SlideMove(false, true, true, true);
    }
    public void AirMove()
    {
        Vector3 wishvel = Vector3.zero;
        Vector3 wishdir = Vector3.zero;
        float wishspeed;
        float scale;
        Friction();

        wishvel = inputDir;
        wishvel = wishvel - Vector3.Dot(wishvel, gravityVector.normalized) * gravityVector.normalized;
        wishdir = wishvel;
        wishspeed = playerSpeed;
        wishdir.y = 0;
        wishdir.Normalize();

        Accelerate(wishdir, wishspeed, PM_AIRACCELERATE);

        if (groundPlane)
        {

        }

        SlideMove(true, false, false, false);
    }
    void SlideMove(bool gravity, bool stepUp, bool stepDown, bool push)
    {
        Vector3 end, stepEnd, primal_velocity, endVelocity, endClipVelocity, clipVelocity;
        float time_left;
        if (gravity)
        {
            endVelocity = current.velocity + gravityVector * frametime;
            current.velocity = (current.velocity + endVelocity) * 0.5f;
        }
        else
        {
            endVelocity = current.velocity;
        }
        time_left = frametime;
        if (gravity)
        {
            current.velocity = endVelocity;
        }
        clipVelocity = current.velocity - Vector3.Dot(gravityVector.normalized, current.velocity) * gravityVector.normalized;
        endClipVelocity = endVelocity - Vector3.Dot(gravityVector.normalized, endVelocity) * gravityVector.normalized;
        if (Vector3.Dot(clipVelocity, endClipVelocity) < 0.0f)
        {
            current.velocity = Vector3.Dot(gravityVector.normalized, current.velocity) * gravityVector.normalized;
        }

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
            vel = vel + Vector3.Dot(vel, gravityVector.normalized) * gravityVector.normalized;
        }
        speed = vel.magnitude;
        if (speed < 1.0f)
        {
            if (Mathf.Abs(Vector3.Dot(current.velocity, gravityVector.normalized)) < 1e-5f)
            {
                current.velocity = Vector3.zero;
            }
            else
            {
                current.velocity = Vector3.Dot(current.velocity, gravityVector.normalized) * gravityVector.normalized;
            }

            return;
        }
        drop = 0;
        if (walking)
        {
            if (!((current.movementFlags & PMF_TIME_KNOCKBACK) != 0))
            {
                control = speed < PM_STOPSPEED ? PM_STOPSPEED : speed;
                drop += control * PM_FRICTION * frametime;
            }
        }
        else
        {
            drop += speed * PM_AIRFRICTION * frametime;
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
        int i, contents;
        Vector3 point = Vector3.zero;
        bool hadGroundContacts;

        Ray ray = new Ray(current.origin, Vector3.down);

        bool IsHit = Physics.Raycast(ray, 2);
        hadGroundContacts = !IsHit;
        contacts = IsHit;
        float fraction = 0;
        if (contacts)
        {

        }
        else
        {
            fraction = 1;
        }

        if (fraction == 1)
        {
            groundPlane = false;
            walking = false;
            return;
        }

        if (Vector3.Dot(current.velocity, -gravityVector.normalized) > 0)
        {
            groundPlane = false;
            walking = false;
            return;
        }
        groundPlane = true;
        walking = true;
        if ((current.movementFlags & PMF_TIME_WATERJUMP) > 0)
        {
            current.movementFlags &= ~(PMF_TIME_WATERJUMP | PMF_TIME_LAND);
            current.movementTime = 0;
        }
        if (!hadGroundContacts)
        {
            if ((Vector3.Dot(current.velocity, -gravityVector.normalized)) < -200.0f)
            {
                current.movementFlags |= PMF_TIME_LAND;
                current.movementTime = 250;
            }
        }
    }
    public bool CheckJump()
    {
        Vector3 addVelocity;

        if (upmove < 1)
        {
            return false;
        }

        if ((current.movementFlags & PMF_JUMP_HELD) > 0)
        {
            return false;
        }

        groundPlane = false;
        walking = false;
        current.movementFlags |= PMF_JUMP_HELD | PMF_JUMPED;

        //addVelocity = 2.0f * maxJumpHeight * -gravityVector;

        //addVelocity= - gravityVector.normalized * Mathf.Sqrt(2.0f * maxJumpHeight * gravityVector.magnitude);

        addVelocity = 2.0f * maxJumpHeight * -gravityVector;

        float man2 = addVelocity.magnitude;
        addVelocity.Normalize();
        addVelocity *= Mathf.Sqrt(man2);
        current.velocity = addVelocity;
        return true;
    }
    public float CmdScale()
    {
        int max;
        float total;
        float scale;
        int forwardmove;
        int rightmove;
        int upmove;

        if (walking)
        {
            upmove = 0;
        }
        else
        {
            upmove = this.upmove;
        }
        return 0;
    }
}
