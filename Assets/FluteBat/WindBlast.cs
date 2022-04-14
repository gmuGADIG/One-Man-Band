using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBlast : MonoBehaviour
{
    private Vector2 movement;
    private Rigidbody2D rb;
    [Range(1.0f, 20.0f)]
    public float moveSpeed;
    private GameObject comeFrom;
    private string Color;

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

    public void setColor(GameObject parent, string Color)
    {
        comeFrom = parent;
        this.Color = Color;
    }

    void move()
    {
        transform.position += (Vector3.Normalize(movement) * -moveSpeed) * Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.Equals(comeFrom)) { }
        else if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
