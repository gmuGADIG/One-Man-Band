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
		//player = GameObject.FindGameObjectWithTag("Player").transform; 
		// in the future try not to push stuff with errors pl0x&th0x :) - David
    }

	// Update is called once per frame
	public void Update()
    {
		base.Update();
        Vector2 direction = Target.transform.position - transform.position;
        rb.rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        direction.Normalize();
        movement = direction;
    }
    // Update enemy movement following convert to Vector2
    // Note: Add acceleration
    private void FixedUpdate() {
        moveCharacter(movement);
    }
	void moveCharacter(Vector2 direction)
	{
		rb.MovePosition((Vector2)transform.position + (direction * movespeed * Time.deltaTime));
	}
}
