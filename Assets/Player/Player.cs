using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /* The Rigidbody and Collider enable us to use the built-in physics system for collisions. */
    private Rigidbody2D rigidbody;
    private Collider2D collider;

    private Animator animator;

    // MUST BE SET IN INSPECTOR!!
    [SerializeField]
    public Transform sprite = null;

    /* The player's top speed, at least under normal conditions. */
    public float maxVelocity = 10;

    /* How fast the player accelerates to the top speed. */
    public float maxAcceleration = 5;

    /* heightOffGround and zVelocity control the player's visual ability to jump. */
    private float heightOffGround = 0;
    private float zVelocity = 0;

    /* The amount of gravity that applies to the player's jump. */
    public float zGravity = 40;

    /* The impulse used for the player's jump. */
    public float zJumpImpulse = 20;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        animator = sprite.GetComponent<Animator>();
    }

    /// <summary>
    /// Gets a normalized vector representing the player's desired direction. May
    /// be modified to take into account joystick controls.
    /// </summary>
    /// <returns></returns>
    private Vector2 getInput()
    {
        Vector2 result = new Vector2();

        // Add each input to the result vector.
        if (Input.GetKey(KeyCode.A)) { result.x -= 1; }
        if (Input.GetKey(KeyCode.D)) { result.x += 1; }
        if (Input.GetKey(KeyCode.S)) { result.y -= 1; }
        if (Input.GetKey(KeyCode.W)) { result.y += 1; }

        // TODO: also add joystick inputs..?

        // Important: this vector must be normalized.
        return result.normalized;
    }

    /// <summary>
    /// Performs the primary physical movement of the player--in particular, accelerating in the
    /// X and Y directions (but does not handle the jumping and its Z component).
    /// </summary>
    private void doXYPhysics()
    {
        Vector2 input = getInput();

        // The targetVelocity is the velocity we would get if we had infinite acceleration.
        Vector2 targetVelocity = input * maxVelocity;

        // The maximum acceleration we can perform this frame is in totalAccel, as it represents
        // an acceleration to the exact target velocity.
        Vector2 totalAccel = (targetVelocity - rigidbody.velocity);

        // Compute the actual acceleration, and multiply by fixedDeltaTime. This enables clamping
        // the delta-multiplied acceleration if it exceeds the maximum.
        Vector2 accel = (totalAccel.normalized * maxAcceleration) * Time.fixedDeltaTime;

        if (accel.magnitude > totalAccel.magnitude)
        {
            // If the accel magnitude is too large, then we only want to accelerate up to
            // totalAccel length. So, we just copy the value over.
            accel = totalAccel;
        }

        // This is essentially equivalent to velocity += accel (which is, remember, already multiplied by fixedDeltaTime)
        // So it is integrating to find velocity, but doing it with a rigidbody enables collisions.
        rigidbody.AddForce(accel, ForceMode2D.Impulse);
    }
    
    /// <summary>
    /// Performs the physics controlling the player's heightOffGround variable.
    /// </summary>
    private void doHeightPhysics()
    {
        if (Input.GetKey(KeyCode.Space) && heightOffGround <= 0)
        {
            zVelocity = zJumpImpulse;
        }

        // Integrate gravity and velocity to get position.
        zVelocity -= zGravity * Time.fixedDeltaTime;
        heightOffGround += zVelocity * Time.fixedDeltaTime;


        // Extremely simple collision--can't be less than 0 height off the ground.
        if(heightOffGround <= 0)
        {
            zVelocity = 0;
            heightOffGround = 0;
        }

        // Whether we collide with walls is based on the height off ground.
        // TODO: Enable collisions that still happen while in the air
        collider.enabled = (heightOffGround <= 0.2);
    }

    private void FixedUpdate()
    {
        doXYPhysics();
        doHeightPhysics();

        // The sprite is moved to the heightOffGround in order to visually represent jumping.
        sprite.localPosition = new Vector3(0, heightOffGround, 0);

        animator.SetFloat("velocity_x", rigidbody.velocity.x);
        animator.SetFloat("velocity_y", rigidbody.velocity.y);
        animator.SetFloat("velocity_len", rigidbody.velocity.magnitude);
    }
}
