using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigidbody;

    public float maxVelocity = 10;
    public float maxAcceleration = 5;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
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

    private void FixedUpdate()
    {
        Vector2 input = getInput();

        Vector2 targetVelocity = input * maxVelocity;

        Vector2 totalAccel = (targetVelocity - rigidbody.velocity);
        Vector2 accel = (totalAccel.normalized * maxAcceleration) * Time.fixedDeltaTime;

        if(accel.magnitude > totalAccel.magnitude)
        {
            accel = totalAccel;
        }

        rigidbody.AddForce(accel, ForceMode2D.Impulse);
    }
}
