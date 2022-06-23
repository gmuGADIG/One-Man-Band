using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : BaseEnemy
{
    [Range(0.1f, 1.0f)]
    public float speed;
    [Range(1.0f, 10.0f)]
    public float angleSpeed;
    [Range(1.0f, 10.0f)]
    public float radius;
    [Range(5.0f,30.0f)]
    public float alertDistance;
    private Rigidbody2D rb;
    private Vector2 movement;
    private float angle;
    public GameObject WindBlast;
    private int FrameAttack;
    public bool attackAnim, doAttack;
    public Animator ani;
    public bool following;
    public AudioClip[] idleClips;
    public AudioClip chargeClip;
    public AudioClip attackClip;
    public AudioClip[] defeatAudio;


    void Start()
    {
        following = false;
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        base.Start();
    }
    
    void FixedUpdate()
    {
        if(Vector2.Distance(transform.position,Target.transform.position) < alertDistance)
        {
            following = true;
        }
        ani.SetBool("Following", following);
        if (following) {
            //angle = Mathf.Atan2(transform.position.x - Target.transform.position.x, transform.position.y - Target.transform.position.y) * -Mathf.Rad2Deg;
            //transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle));
            //Frame Timing of the bats cycle, Attacks after this many frames
            //The float being added to radius is the offset for tracking
            ani.SetBool("Attack", attackAnim);
            if ((Vector2.Distance(transform.position, Target.transform.position) < radius + 0.2) && !attackAnim)
            {
                attackAnim = true;
                FrameAttack = 0;
            }
            else if (!attackAnim)
            {
                MoveToward();
            }
       
        }
    }
    //Moves toward specified target
    void MoveToward()
    {
        rb.MovePosition(Vector2.MoveTowards(transform.position, Target.transform.position, speed));
    }
    public void Charge()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(chargeClip);
    }
    //Makes an attack square
    public void Attack()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(attackClip);
        GameObject temp = Instantiate(WindBlast, transform.position, transform.rotation);
        temp.GetComponent<WindBlast>().setMovement(new Vector3(transform.position.x - Target.transform.position.x, transform.position.y - Target.transform.position.y, 0.0f), Mathf.Atan2(Target.transform.position.x - transform.position.x, Target.transform.position.y - transform.position.y) * -Mathf.Rad2Deg);
        temp.GetComponent<WindBlast>().setColor(gameObject, "Red");
        attackAnim = false;
    }
}
