using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteChest : MonoBehaviour
{
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
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(disperse());
        }
    }

    IEnumerator disperse()
    {
        float[] radius = new float[transform.childCount];
        float[] angle = new float[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
            radius[i] = Random.Range(0.05f, 0.12f);
            angle[i] = Random.Range(-1f, 1f)*20f*Mathf.Deg2Rad; 
        }
        float timer = Time.time; 
        while (Time.time - timer < 0.1f)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).Translate(
                    radius[i] * Mathf.Cos(angle[i] + 2*Mathf.PI * i / transform.childCount), 
                    radius[i] * Mathf.Sin(angle[i] + 2*Mathf.PI * i / transform.childCount), 
                    0);

            }
            yield return null;
        }
        transform.DetachChildren();
        Destroy(gameObject);
    }
}
