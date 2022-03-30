using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentNote : MonoBehaviour
{
    public static int notesCollected;

    public delegate void NoteEvent();
    public event NoteEvent Collected;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void Collect()
    {
        notesCollected++;
        Collected?.Invoke();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            Collect();
        }
    }
}