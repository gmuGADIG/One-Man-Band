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
<<<<<<< HEAD:Assets/LuteOrc/Orc.cs
		base.Update();
        Vector3 direction = Target.transform.position - transform.position;
        //Debug.Log(direction);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // i assume your IDE isnt doing autocomplete so maybe do some googling on how to fix that lol - David
        rb.rotation = angle;
        direction.Normalize();
        movement = direction;
=======
        base.Update();
        movement = (Target.transform.position - transform.position).normalized; 
        rb.rotation = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
>>>>>>> e1d6aadb88ef1ac58d07180e13ad24bf8f6921a3:Assets/Scripts/Orc.cs
    }

    // Note: Add acceleration
    private void FixedUpdate() {
        if (Target)
        {
            float targetDist = Vector3.Distance(Target.transform.position, transform.position);
            if (targetDist > 2)
            {
                moveCharacter();
            }
        }
    }
	void moveCharacter()
	{
		rb.MovePosition((Vector2)transform.position + (movement * movespeed * Time.deltaTime));
	}
}
