using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GameModes, Definitions and Values


// Cube: Simple jumping cube, rotates 90 on jump
// initial velocity = 19.5269f
// gravity scale = 9.057f
// rotationMod = 409.1f
// OnGround = true
// Camera = FreeCam


// Ship: Fly up while holding action, drop down with !action
// OnGround = false
// Camera = Static


// Ball: Slow flip to opposite side with gravity scale, like dropping a ball
// gravity scale = 6.2f
// OnGround = true
// Camera = Static


// UFO: Fly up with click, drop down with !click
// initial velocity = 10.841f
// gravity scale = 4.1483f
// velocity limit = 10.841f
// OnGround = false
// Camera = Static


// Wave: has a 1:1 on incline and decline
// Camera = Static


// Robot: 
// Camera = FreeCam


// Spider: Fast flip to opposite side, mechanical feel
// initial velocity = 238.29f
// gravity scale = 6.2f
// velocity limit = 238.29f
// OnGround = true
// Camera = Static


public enum Gamemodes { Cube = 0, Ship = 1, Ball = 2, UFO = 3, Wave = 4, Robot = 5, Spider = 6};
public enum Speeds { Slow = 0, Normal = 1, Fast = 2, Faster = 3, Fastest = 4 };

public class Movement : MonoBehaviour
{
    public Speeds CurrentSpeed;
    public Gamemodes CurrentGamemode;
    //                       0      1      2       3      4
    float[] SpeedValues = { 8.6f, 10.4f, 12.96f, 15.6f, 19.27f };
    //                                  0   1   2   3   4   5  6
    [System.NonSerialized] public int[] screenHeightValues = { 11, 10, 8, 10, 10, 11, 9 };
    [System.NonSerialized] public float yLastPortal;

    public float GroundCheckRadius;
    public LayerMask GroundMask;
    public Transform Sprite;

    Rigidbody2D rb;

    public int Gravity = 1;
    public bool clickProcessed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        transform.position += Vector3.right * SpeedValues[(int)CurrentSpeed] * Time.deltaTime;

        Invoke(CurrentGamemode.ToString(), 0);
    }

    public bool OnGround()
    {
        return Physics2D.OverlapBox(transform.position + Vector3.down * Gravity * 0.5f, Vector2.right * 1.1f + Vector2.up * GroundCheckRadius, 0, GroundMask);
    }

    bool TouchingWall()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + (Vector2.right * 0.55f), Vector2.up * 0.8f + (Vector2.right * GroundCheckRadius), 0, GroundMask);
    }

    void Cube()
    {
        generic.CreateGameMode(rb, this, true, 19.5269f, 9.057f, true, false, 409.1f);
    }

    void Ship()
    {
        rb.gravityScale = 2.93f * (Input.GetMouseButton(0) ? -1 : 1) * Gravity;
        generic.LimitYVelocity(9.95f, rb);
        transform.rotation = Quaternion.Euler(0, 0, rb.velocity.y * 2);
    }

    void Ball()
    {
        generic.CreateGameMode(rb, this, true, 0, 6.2f, false, true);
    }

    void UFO()
    {
        generic.CreateGameMode(rb, this, false, 10.841f, 4.1483f, false, false, 0, 10.841f);
    }

    void Wave()
    {
        rb.gravityScale = 0;
        rb.velocity = new Vector2(0, SpeedValues[(int)CurrentSpeed] * (Input.GetMouseButton(0) ? 1 : -1) * Gravity);
    }

    void Spider()
    {
        generic.CreateGameMode(rb, this, true, 238.29f, 6.2f, false, true, 0, 238.29f);
    }

    float robotXstart = -100;
    bool onGroundProcessed;
    bool gravityFlipped;

    void Robot()
    {
        if (!Input.GetMouseButton(0))
            clickProcessed = false;

        if (OnGround() && !clickProcessed && Input.GetMouseButton(0))
        {
            gravityFlipped = false;
            clickProcessed = true;
            robotXstart = transform.position.x;
            onGroundProcessed = true;
        }

        // checking distance of current x value and the x value of first frame
        if (Mathf.Abs(robotXstart - transform.position.x) <= 3)
        {
            if (Input.GetMouseButton(0) && onGroundProcessed && !gravityFlipped)
            {
                rb.gravityScale = 0;
                rb.velocity = Vector2.up * 10.4f * Gravity;
                return;
            }
        }
        else if (Input.GetMouseButton(0))
            onGroundProcessed = false;

        rb.gravityScale = 8.62f * Gravity;
        generic.LimitYVelocity(23.66f, rb);
    }



    public void ChangeThroughPortal(Gamemodes Gamemode, Speeds Speed, int gravity, int State, float _yPortal)
    {
        switch (State)
        {
            case 0:
                CurrentSpeed = Speed;
                break;
            case 1:
                yLastPortal = _yPortal;
                CurrentGamemode = Gamemode;
                break;
            case 2:
                Gravity = gravity;
                rb.gravityScale = Mathf.Abs(rb.gravityScale) * gravity;
                gravityFlipped = true;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PortalScript portal = collision.gameObject.GetComponent<PortalScript>();
        if (portal)
            portal.InitiatePortal(this);
    }
}
