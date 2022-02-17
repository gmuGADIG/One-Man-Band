using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 velocity;

    public float maxVelocity = 10;
    public float maxAcceleration = 5;

    private void Start()
    {

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

        Vector2 totalAccel = (targetVelocity - velocity);
        Vector2 accel = (totalAccel.normalized * maxAcceleration) * Time.fixedDeltaTime;

        if(accel.magnitude > totalAccel.magnitude)
        {
            accel = totalAccel;
        }

        velocity += accel;

        transform.Translate((Vector3)velocity * Time.fixedDeltaTime);
    }
}
