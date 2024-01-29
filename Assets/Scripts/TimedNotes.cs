using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedNotes : ParentNote
{
    DaddyNote Dn;
    // Start is called before the first frame update
    void Start()
    {
        Dn = transform.parent.GetComponent<DaddyNote>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
			AudioSource.PlayClipAtPoint(pickupSound[Random.Range(0,pickupSound.Length)], transform.position);
			Dn.StartTracking();
            gameObject.SetActive(false);
        }
    }
}
