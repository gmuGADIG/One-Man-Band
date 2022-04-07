using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBlast : MonoBehaviour
{
    private Vector2 movement;
    private Rigidbody2D rb;
    [Range(1.0f, 10.0f)]
    public float moveSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        move();
    }

    public void setMovement(Vector2 move)
    {
        movement = move;
    }

    void move()
    {
        transform.position += (Vector3.Normalize(movement) * -moveSpeed) * Time.deltaTime;
    }
}
