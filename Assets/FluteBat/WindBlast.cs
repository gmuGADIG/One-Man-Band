using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBlast : MonoBehaviour
{
    private Vector2 movement;
    private Rigidbody2D rb;
    [Range(1.0f, 50.0f)]
    public float moveSpeed;
    [Range(0.5f,10)]
    [Tooltip("Time in Seconds")]
    public float timeToDisappear;
    private GameObject comeFrom;
    private string Color;
    private int frameCounter;

    void Start()
    {
        timeToDisappear *= 50;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (frameCounter > timeToDisappear)
            Destroy(gameObject);
        move();
        frameCounter++;
    }

    public void setMovement(Vector2 move, float rot)
    {
        movement = move;
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, rot));
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
            Health health = collision.gameObject.GetComponent<Health>();
            if(health!=null){
                health.Damage(1);
            }
            Destroy(gameObject);
        }
    }
}
