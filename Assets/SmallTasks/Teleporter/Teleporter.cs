using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private string validTag = "Player";
    [SerializeField] private Transform location;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == validTag)
        {
            collision.gameObject.transform.position = location.position;
        }
    }
}
