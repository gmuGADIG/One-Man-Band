using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluteBat : BaseEnemy
{
    public Transform target;
    [Range(1.0f, 10.0f)]
    public float speed;
    [Range(1.0f, 10.0f)]
    public float radius;
    private Rigidbody2D rb;
    private Vector2 movement;
    private float angle;
    public GameObject WindBlast;
    public int FrameAttack = 0;
    public bool attackAnim, doAttack;
    public Animator ani;


    void Start()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }


    void FixedUpdate()
    {
        
        if (FrameAttack >= 100)
        {
            attackAnim = true;
        }
        if (doAttack)
        {
            Attack();
        }
        if ((Vector2.Distance(transform.position, target.position) > radius + 0.01) && !attackAnim)
        {
            MoveToward();
            FrameAttack = 0;
        }
        else if (!attackAnim)
        {
            TargetMove();
        }
        FrameAttack++;
        ani.SetBool("AttackAnim", attackAnim);
    }
    void MoveToward()
    {
        rb.MovePosition(Vector2.MoveTowards(transform.position, target.position, speed/20));
        angle = Mathf.Atan2(transform.position.x - target.position.x, transform.position.y - target.position.y) * -Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle));
    }

    void TargetMove()
    {
        angle += speed;

        movement.x = (-Mathf.Sin(angle * Mathf.Deg2Rad) * radius) + target.position.x;
        movement.y = (Mathf.Cos(angle * Mathf.Deg2Rad) * radius) + target.position.y;

        rb.MovePosition(movement);

        transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle));
    }

    void Attack()
    {
        GameObject temp = Instantiate(WindBlast, transform.position, transform.rotation);
        temp.GetComponent<WindBlast>().setMovement(new Vector3(transform.position.x - target.position.x, transform.position.y - target.position.y, 0.0f));
        temp = null;
        FrameAttack = 0;
        doAttack = false;
        attackAnim = false;
    }

}
