using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Speeds { Slow = 0, Normal = 1, Fast = 2, Faster = 3, Fastest = 4};

public class Movement : MonoBehaviour
{

    public Speeds CurrentSpeed;

    float[] speedValues = { 8.6f, 10.4f, 12.96f, 15.6f, 19.27f };

   
    public Transform groundCheckTransform;
    public float groundCheckRadius;
    public LayerMask groundMask;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // position and speed
        transform.position += Vector3.right * speedValues[(int)CurrentSpeed] * Time.deltaTime;

        // jump if grounded
        if (OnGround() && Input.GetMouseButton(0))
        {
            print("Jumping");
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * 26.6581f, ForceMode2D.Impulse);
        }
    }

    bool OnGround()
    {
        return Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundMask);
    }
}
