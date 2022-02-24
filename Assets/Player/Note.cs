using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    //Notes traker
    public int notesCollected;
    public GameObject notes;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void OnTriggerEnter2D(Collider2D col)
    {       
       if(col.gameObject.tag == "Note")
        {
            GameObject.Destroy(col.gameObject);
            notesCollected++;
        }
    }
}
