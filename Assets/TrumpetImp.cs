using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpetImp : MonoBehaviour
{
    /* The Rigidbody enables us to use the built-in physics system for collisions. */
    private Rigidbody2D rigidbody;

    /* The enemy's top speed, at least under normal conditions. */
    public float maxVelocity = 10;

    /* How fast the enemy accelerates to the top speed. */
    public float maxAcceleration = 5;

    private Vector2 targetLocation;

    private GameObject currentTargetObject = null;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        currentTargetObject = GameObject.FindWithTag("Player");

    }

    /// <summary>
    /// Performs the primary physical movement of the player--in particular, accelerating in the
    /// X and Y directions (but does not handle the jumping and its Z component).
    /// </summary>
    private void doXYPhysics()
    {
        Vector2 difference = (targetLocation - rigidbody.position);
        Vector2 targetVelocity = difference.normalized * maxVelocity;

        float decelThreshold = rigidbody.velocity.sqrMagnitude / maxAcceleration;
        if (difference.magnitude < decelThreshold)
        {
            Vector2 accel = -rigidbody.velocity.normalized * maxAcceleration;
            accel *= Time.fixedDeltaTime;

            float maxFrameAccel = rigidbody.velocity.magnitude;
            if(accel.magnitude > maxFrameAccel)
            {
                accel = accel.normalized * maxFrameAccel;
            }

            rigidbody.AddForce(accel, ForceMode2D.Impulse);
            if(rigidbody.velocity.sqrMagnitude < 0.001 * 0.001)
            {
                rigidbody.velocity = new Vector2(0, 0);
            }
        }
        else
        {
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
    }

    private void computeTarget()
    {
        if(currentTargetObject != null)
        {
            targetLocation = (Vector2)currentTargetObject.transform.position;
        }
    }

    private void FixedUpdate()
    {
        computeTarget();
        doXYPhysics();
    }
}
