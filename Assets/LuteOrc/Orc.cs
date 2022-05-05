using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : BaseEnemy
{

    public float movespeed = 2f, minDist = 1;
	[SerializeField] int damage = 1;
	[SerializeField] int coolDown = 1;
    private Rigidbody2D rb;
    private Vector2 movement;
    public GameObject player;
    public GameManager gm;
    public SpriteRenderer sr;
    public Animator am;

    // Start is called before the first frame update
    public void Start()
    {
		base.Start();
        gm = (GameManager) GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        player = gm.GetPlayer();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        am = GetComponent<Animator>();
    }

	// Update is called once per frame
	public void Update()
    {
        base.Update();
        if (Target)
        {
            movement = (Target.transform.position - transform.position).normalized;
            //rb.rotation = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            if (transform.position.x >= Target.transform.position.x)
            {
                //face left
                sr.flipX = false;
            }
            else
            {
                //face right
                sr.flipX = true;
            }
        }
    }
	float timer = 0;
    // Note: Add acceleration
    private void FixedUpdate() {
        if (Target)
        {
            float targetDist = Vector3.Distance(Target.transform.position, transform.position);
            if (targetDist > minDist)
            {
                am.SetBool("isAttack", false);
                moveCharacter();
            }
            else
            {
                transform.GetChild(0).GetComponent<OgreHurtbox>().SetTarget(Target);
                am.SetBool("isAttack", true);
				/*timer += Time.deltaTime;
				print(timer);
				if (timer >= coolDown)
				{
					Target.GetComponent<Health>().Damage(damage);
					timer = 0;
				}*/
            }
        }

    }
	void moveCharacter()
	{
		rb.MovePosition((Vector2)transform.position + (movement * movespeed * Time.deltaTime));
	}
}
