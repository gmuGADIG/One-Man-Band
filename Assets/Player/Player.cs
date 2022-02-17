using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 velocity;

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

        velocity = input * 1;

        transform.Translate((Vector3)velocity);
    }
}
