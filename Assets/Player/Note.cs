using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : ParentNote
{
    //Notes traker
    
    public GameObject notes;
    public GameObject[] timedNotes;// = new GameObject[3];
    // Start is called before the first frame update
    public TimedNoteManager tmd;
    void Start()
    {
        for (int i = 0; i < timedNotes.Length; i++)
        {
            timedNotes[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

        //StartCoroutine(coroutineA());
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "TimedNotes")
        {
            col.gameObject.SetActive(false);
            notesCollected++;

            if (!tmd.check)
            {
                StartCoroutine(keepTrack());
                tmd.check = true;
            }

        }
        if (col.gameObject.tag == "Daddy")
        {
            GameObject.Destroy(col.gameObject);
            notesCollected++;

            for (int i = 0; i < timedNotes.Length; i++)
            {
                timedNotes[i].SetActive(true);
            }

        }
    }

    IEnumerator keepTrack()
    {
        yield return new WaitForSeconds(3.0f);
        Debug.Log("THREE SECONFSDD");
        bool check = false;
        for (int i = 0; i < timedNotes.Length; i++)
        {
            if (timedNotes[i].activeSelf)
            {
                check = true;
                break;
            }
        }
        if (check)
        {
            for (int i = 0; i < timedNotes.Length; i++)
            {
                timedNotes[i].SetActive(true);
                tmd.check = false;
            }
        } else
        {
            Collect();
        }
    }
}
