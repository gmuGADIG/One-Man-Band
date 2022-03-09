using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZone : MonoBehaviour
{
    [SerializeField] private Vector2 direction;
    [SerializeField] private float windStrength = 4f;
    [SerializeField] private bool isBlowing = false;

    private List<Rigidbody2D> bodies = new List<Rigidbody2D>();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (direction.magnitude <= 1)
            Gizmos.DrawRay(transform.position, new Vector3(direction.x, direction.y, transform.position.z));
        else
            Gizmos.DrawRay(transform.position, new Vector3(direction.x, direction.y, transform.position.z).normalized);
    }
    private void Start()
    {
        direction.Normalize();

        //These test to see if it should blow on start and plays any visual effects
        if (isBlowing)
            TurnOnWind();
        else
            TurnOffWind();

    }
    private void FixedUpdate()
    {
        if (isBlowing)
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
    public void TurnOnWind()
    {
        isBlowing = true;
        //start any wind Particles or graphic animations
    }
    public void TurnOffWind()
    {
        isBlowing = false;
        // stop wind particles or animations
    }
}
