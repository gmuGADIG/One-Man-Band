using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteChest : MonoBehaviour
{
    private IEnumerable disperseCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<Notes>() != null)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject drop = transform.GetChild(i).gameObject;
                drop.SetActive(true);
                //StartCoroutine(disperse());
            }
            transform.DetachChildren();
            Destroy(gameObject);
        }
    }
}
