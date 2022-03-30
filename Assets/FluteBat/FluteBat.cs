using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluteBat : BaseEnemy
{
    public Transform target;
    [Range(1.0f, 10.0f)]
    public float speed;
    [Range(1.0f, 10.0f)]
    public float radius;
    private Rigidbody2D rb;
    private Vector2 movement;
    private float angle;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }


    void FixedUpdate()
    {
            move();

    }

    void move()
    {
        angle += speed;

        movement.x = (-Mathf.Sin(angle * Mathf.Deg2Rad) + target.position.x) * radius;
        movement.y = (Mathf.Cos(angle * Mathf.Deg2Rad) + target.position.y) * radius;

        rb.MovePosition(movement);

        transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle));
    }

}
