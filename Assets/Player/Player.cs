using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private Collider2D collider;

    // MUST BE SET IN INSPECTOR!!
    [SerializeField]
    public Transform sprite = null;

    public float maxVelocity = 10;
    public float maxAcceleration = 5;

    private float heightOffGround = 0;

    private float zVelocity = 0;

    public float zGravity = 40;
    public float zJumpImpulse = 20;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    private Vector2 getInput()
    {
        Vector2 result = new Vector2();

        if(Input.GetKey(KeyCode.A))
        {
            result.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            result.x += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            result.y -= 1;
        }
        if (Input.GetKey(KeyCode.W))
        {
            result.y += 1;
        }

        return result.normalized;
    }

    private void doGroundPhysics()
    {
        Vector2 input = getInput();

        Vector2 targetVelocity = input * maxVelocity;

        Vector2 totalAccel = (targetVelocity - rigidbody.velocity);
        Vector2 accel = (totalAccel.normalized * maxAcceleration) * Time.fixedDeltaTime;

        if (accel.magnitude > totalAccel.magnitude)
        {
            accel = totalAccel;
        }

        rigidbody.AddForce(accel, ForceMode2D.Impulse);
    }

    private void doHeightPhysics()
    {
        if (Input.GetKey(KeyCode.Space) && heightOffGround <= 0)
        {
            zVelocity = zJumpImpulse;
        }

        zVelocity -= zGravity * Time.fixedDeltaTime;
        heightOffGround += zVelocity * Time.fixedDeltaTime;

        if(heightOffGround <= 0)
        {
            zVelocity = 0;
            heightOffGround = 0;
        }

        collider.enabled = (heightOffGround <= 0.2);
    }

    private void FixedUpdate()
    {
        doGroundPhysics();
        doHeightPhysics();

        sprite.localPosition = new Vector3(0, heightOffGround, 0);
    }
}
