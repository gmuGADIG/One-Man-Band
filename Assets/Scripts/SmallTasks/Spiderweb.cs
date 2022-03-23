using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spiderweb : MonoBehaviour
{
    [SerializeField] private float maxSpeedinWeb = 2f;

    //private List<character> effectedCharacters = new List<Character>();
    private List<Rigidbody2D> bodies = new List<Rigidbody2D>();

    private void FixedUpdate()
    {
        foreach(Rigidbody2D rb in bodies)
        {
            if (rb.velocity.magnitude >= maxSpeedinWeb)
                rb.velocity = rb.velocity.normalized * maxSpeedinWeb;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb;
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            //add to character list
        }
        else if (collision.gameObject.TryGetComponent(out rb))
        {
            if (!bodies.Contains(rb))
            {
                bodies.Add(rb);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Rigidbody2D rb;
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            //add to character list
        }
        else if (collision.gameObject.TryGetComponent(out rb))
        {
            if (bodies.Contains(rb))
            {
                bodies.Remove(rb);
            }
        }
    }
}
/*
 * Im guessing the player and the enemies are not handled by physics
 * so i am just leaving it empty for now, but anything controlled by physics will be affected
 */