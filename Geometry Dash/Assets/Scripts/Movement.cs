using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Speeds { Slow = 0, Normal = 1, Fast = 2, Faster = 3, Fastest = 4};
public enum GameModes { Cube = 0, Ship = 1 };
public enum Gravity { Upright = 1, UpsideDown = -1 };

public class Movement : MonoBehaviour
{

    public Speeds currentSpeed;
    public GameModes currentGamemode;
    float[] speedValues = { 8.6f, 10.4f, 12.96f, 15.6f, 19.27f };

   
    public Transform groundCheckTransform;
    public float groundCheckRadius;
    public LayerMask groundMask;

    public Transform sprite;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // position and speed
        transform.position += Vector3.right * speedValues[(int)currentSpeed] * Time.deltaTime;

        // jump if grounded
        if (OnGround())
        {
            #region control rotation
            // control rotation so player lands flat and not at an angle
            Vector3 rotation = sprite.rotation.eulerAngles; // converts Quaternion into vector 3 to round up
            rotation.z = Mathf.Round(rotation.z / 90) * 90; // rounds rotation up
            sprite.rotation = Quaternion.Euler(rotation); // converts Vector 3 back into Quaternion
            #endregion

            if (Input.GetMouseButton(0))
            {
                print("Jumping");
                rb.velocity = Vector2.zero;
                rb.AddForce(Vector2.up * 26.6581f, ForceMode2D.Impulse);
            }
        }
        else
        {
            // rotate sprite not the RB or it would misbahve with box collider and RB
            sprite.Rotate(Vector3.back * 5);
        }
    }

    bool OnGround()
    {
        return Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundMask);
    }

    public void ChangeThroughPortal(GameModes GameMode, Speeds Speed, Gravity Gravity, int State)
    {
        switch (State)
        {
            case 0:
                currentSpeed = Speed;
                break;
            case 1:
                currentGamemode = GameMode;
                break;
            case 2:
                rb.gravityScale = Mathf.Abs(rb.gravityScale) * (int)Gravity;
                break;
        }
    }
}
