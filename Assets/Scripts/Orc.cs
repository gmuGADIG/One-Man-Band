using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : BaseEnemy
{
    public float movespeed = 2f;
    private Rigidbody2D rb;
    private Vector2 movement;

    // Start is called before the first frame update
    public void Start()
    {
		base.Start();
        rb = GetComponent<Rigidbody2D>();
    }

	// Update is called once per frame
	public void Update()
    {
        movement = (Target.transform.position - transform.position).normalized; 
        rb.rotation = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
    }
    // Update enemy movement following convert to Vector2
    // Note: Add acceleration
    private void FixedUpdate() {
        moveCharacter();
    }
	void moveCharacter()
	{
		rb.MovePosition((Vector2)transform.position + (movement * movespeed * Time.deltaTime));
	}
}
