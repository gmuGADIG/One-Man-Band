using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaddyNote : ParentNote
{
    public GameObject[] timedNotes;
    public TimedNoteManager tmd;
    public bool Tracking = false;
    // Start is called before the first frame update
    void Start()
    {
        timedNotes = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            timedNotes[i] = transform.GetChild(i).gameObject;
            timedNotes[i].SetActive(false);
        }
    }

    new protected void Collect()
    {
        for (int i = 0; i < timedNotes.Length; i++)
        {
            notesCollected++;
        }
        Debug.Log(notesCollected);
        Destroy(gameObject);

        //Collected?.Invoke();
    }

    void SpawnNotes()
    {
        notesCollected++;
        for (int i = 0; i < timedNotes.Length; i++)
        {
            timedNotes[i].SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            gameObject.GetComponent<Renderer>().enabled = false;
            SpawnNotes();


        }
        /*if (col.gameObject.tag == "Daddy")
        {
            GameObject.Destroy(col.gameObject);
            notesCollected++;

            for (int i = 0; i < timedNotes.Length; i++)
            {
                timedNotes[i].SetActive(true);
            }

        }*/
    }

    public void StartTracking()
    {
        if (!Tracking)
        {
            StartCoroutine(keepTrack());
            Tracking = true;
        }

    }

    IEnumerator keepTrack()
    {
        Debug.Log("Start Tracking");
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
                
            }
            Tracking = false;
        }
        else
        {
            Collect();
        }
    }
}
