using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZone : MonoBehaviour
{
    [SerializeField] private Vector2 direction;
    [SerializeField] private float windStrength = 4f;

    private List<Rigidbody2D> bodies = new List<Rigidbody2D>();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, new Vector3(direction.x, direction.y, transform.position.z));
    }
    private void Start()
    {
        direction.Normalize();
    }
    private void FixedUpdate()
    {
        foreach (Rigidbody2D body in bodies)
        {
            body.AddForce(direction * windStrength);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb;
        if (collision.gameObject.TryGetComponent(out rb))
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
        if (collision.gameObject.TryGetComponent(out rb))
        {
            if (bodies.Contains(rb))
            {
                bodies.Remove(rb);
            }
        }
    }
    public void ChangeDirection(Vector2 newDir)
    {
        direction = newDir.normalized;
    }
}
