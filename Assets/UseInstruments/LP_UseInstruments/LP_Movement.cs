using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LP_Movement : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    public float jumpHeight;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move() 
    {
        float x = Input.GetAxisRaw("Horizontal");
        float moveBy = x * speed;
        rb.velocity = new Vector2(moveBy, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && onGround()) {
            rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
        }
    }

    bool onGround() {
        return Mathf.Abs(rb.velocity.y) <= 0.00001f;
    }


}
