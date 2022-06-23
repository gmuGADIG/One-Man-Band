using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaddyNote : ParentNote
{
	[Tooltip("Just make some 'TimedNote' prefabs and make them child objects of this and toy wont need to worry about the rest of this stuff")]
    public GameObject[] timedNotes;
    public TimedNoteManager tmd;
    public bool Tracking = false;
	public float TimeLimit = 3;
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
            GameManager.gm.notesCollected++;
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
        if (col.gameObject.tag == "Player" && gameObject.GetComponent<Renderer>().enabled)
        {
			AudioSource.PlayClipAtPoint(pickupSound[Random.Range(0, pickupSound.Length)], transform.position);
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
        yield return new WaitForSeconds(TimeLimit);
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
