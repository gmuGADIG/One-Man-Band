using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Vector3 center;
    public Vector3 size;

    [SerializeField] private string validTag = "Player";
    [SerializeField] private Transform location;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == validTag)
        {
			Health hp = collision.GetComponent<Health>();
			if (hp.canDamage)
			{
				hp.canDamage = false;
				hp.Invoke("resetDamageFlag", 1f);
			}
            collision.gameObject.transform.position = location.position;
        }
    }

    /*private void OnDrawGizmosSelected()
    {
        //center = new Vector3(location.position.x, location.position.y, 0.0f);
        size = new Vector3(2, 2, 2);

        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(location.transform.localPosition, size);
    }*/
}
