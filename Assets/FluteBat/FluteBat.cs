using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluteBat : BaseEnemy
{
    [Range(0.1f, 1.0f)]
    public float speed;
    [Range(1.0f, 10.0f)]
    public float angleSpeed;
    [Range(1.0f, 10.0f)]
    public float radius;
    [Range(5.0f, 30.0f)]
    public float alertDistance;
    private Rigidbody2D rb;
    private Vector2 movement;
    private float angle;
    public GameObject WindBlast;
    private int FrameAttack;
    public bool attackAnim, doAttack;
    public Animator ani;
    public bool following;
    public AudioClip[] idle;
    public AudioClip attackClip;


	float baseSpeed;
	void Start()
    {
		baseSpeed = speed;
        following = false;
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        base.Start();
    }
    
    void FixedUpdate()
    {
		if (Target.tag != "Player")
		{
			speed = baseSpeed * 1.5f;
		}
		else
		{
			speed = baseSpeed;
		}
		following = Vector2.Distance(transform.position, Target.transform.position) < alertDistance;
        ani.SetBool("Follow", following);
        if (following)
        {
            //Frame timing of the bats attack cycle, Stops after this many frames
            if (FrameAttack >= 50)
            {
                attackAnim = true;
            }
            ani.SetBool("Attack", attackAnim);
            //The float being added to radius is the offset for tracking
            if ((Vector2.Distance(transform.position, Target.transform.position) > radius + 0.2) && !attackAnim)
            {
                MoveToward();
                FrameAttack = 0;
            }
            else if (!attackAnim)
            {
				//TargetMove();
				MoveToward();
            }
            FrameAttack++;
        }
    }
    //Moves toward specified target
    void MoveToward()
    {
		angle += angleSpeed;

		movement.x = (-Mathf.Sin(angle * Mathf.Deg2Rad) * radius) + Target.transform.position.x;
		movement.y = (Mathf.Cos(angle * Mathf.Deg2Rad) * radius) + Target.transform.position.y;

		rb.MovePosition(Vector2.MoveTowards(transform.position, movement, speed));
		//angle = Mathf.Atan2(Target.transform.position.x - transform.position.x, Target.transform.position.y - transform.position.y) * Mathf.Rad2Deg;
		//angle = Mathf.Atan2(transform.position.x - Target.transform.position.x, transform.position.y - Target.transform.position.y) * -Mathf.Rad2Deg;
		//transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle));
	}
    //Moves in a perfect circle around the character
    void TargetMove()
    {
		angle += angleSpeed;

		movement.x = (-Mathf.Sin(angle * Mathf.Deg2Rad) * radius) + Target.transform.position.x;
        movement.y = (Mathf.Cos(angle * Mathf.Deg2Rad) * radius) + Target.transform.position.y;
		
        rb.MovePosition(movement);

        //transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle));
    }
    //Makes an attack square
    public void Attack()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(attackClip);
        GameObject temp = Instantiate(WindBlast, transform.position, transform.rotation);
        temp.GetComponent<WindBlast>().setMovement(new Vector3(transform.position.x - Target.transform.position.x, transform.position.y - Target.transform.position.y, 0.0f), Mathf.Atan2(Target.transform.position.x - transform.position.x, Target.transform.position.y - transform.position.y) * -Mathf.Rad2Deg);
        temp.GetComponent<WindBlast>().setColor(gameObject, "Red");
        //angle = Mathf.Atan2(Target.transform.position.x - transform.position.x, Target.transform.position.y - transform.position.y) * Mathf.Rad2Deg;
        attackAnim = false;
        FrameAttack = 0;
    }

}
