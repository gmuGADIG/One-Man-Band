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
    public AudioClip[] attackAudio;
    /*public AudioClip[] hurtAudio;
    public AudioClip[] defeatAudio;*/
    public AudioClip[] idleAudio;
    public float randSound = 1.8f;

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
            sr.flipX = transform.position.x < Target.transform.position.x;
        }
    }

    // Note: Add acceleration
    private void FixedUpdate() {
		float targetDist = Vector3.Distance(Target.transform.position, transform.position);
        if (Target && targetDist < maxDistance)
        {
            am.SetBool("hasTarget", true);
            
            if (targetDist > minDist)
            {
                am.SetBool("isAttack", false);
				moveCharacter();
            }
            else
            {
                transform.GetChild(0).GetComponent<OgreHurtbox>().SetTarget(Target);
                am.SetBool("isAttack", true);
            }
        } else
        {
            am.SetBool("isAttack", false);
            am.SetBool("hasTarget", false);
            rb.velocity = Vector3.zero; //To prevent "sliding"
        }
    }

    public override void Die()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Animator>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        float timer = Time.time;
        GetComponent<AudioSource>().PlayOneShot(defeatAudio[Random.Range(0,defeatAudio.Length)]);
        Invoke("MyDestroy", 2.0f);
    }

    private void MyDestroy()
    {
        Destroy(gameObject);
    }
    /*private void checkConvertNoteCollide(Collider2D collision)
    {
        // Already converted, don't need to check for conversion notes
        if (convertHealth <= 0) return;

        Notes noteScript = collision.gameObject.GetComponent<Notes>();
        // If we actually collided with a note...
        if (noteScript != null)
        {
            gameObject.GetComponent<AudioSource>().PlayOneShot(hurtAudio[Random.Range(0,hurtAudio.Length)]);
            convertHealth -= noteScript.damage;
            if (convertHealth <= 0)
            {
                ChangeAffiliation(EnemyAffiliation.WithPlayer);
            }
        }
    }*/


    private void OnTriggerEnter2D(Collider2D collision)
    {
        checkConvertNoteCollide(collision);
    }
    void moveCharacter()
	{
		rb.MovePosition((Vector2)transform.position + (movement * movespeed * Time.deltaTime));
        randSound -= 1 * Time.deltaTime;
        if (randSound <= 0.0f)
        {
            this.gameObject.GetComponent<AudioSource>().PlayOneShot(idleAudio[Random.Range(0, idleAudio.Length)]);
            randSound = 1.8f;
        }
    }
}
