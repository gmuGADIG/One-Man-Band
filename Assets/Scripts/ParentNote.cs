using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentNote : MonoBehaviour
{
    public static int notesCollected;

    public delegate void NoteEvent();
    public event NoteEvent Collected;
    public GameManager gm;
	[SerializeField] protected AudioClip[] pickupSound;
	// Start is called before the first frame update
	void Start()
    {
        gm = (GameManager)GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void Collect()
    {
        notesCollected++;
        GameManager.gm.notesCollected++;
        GameManager.gm.changeSong();
        Collected?.Invoke();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
			AudioSource.PlayClipAtPoint(pickupSound[Random.Range(0, pickupSound.Length)], transform.position);
			Destroy(gameObject);
            Collect();
        }
    }
}
