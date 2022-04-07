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
            radius[i] = Random.Range(0.01f, 0.05f);
            angle[i] = Random.Range(-20f, 20f); 
        }
        float timer = Time.time; 
        while (Time.time - timer < 0.25f)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).Translate(
                    radius[i] * Mathf.Cos(angle[i] + 360 * i / transform.childCount), 
                    radius[i] * Mathf.Sin(angle[i] + 360 * i / transform.childCount), 
                    0);

            }
            yield return null;
        }
        transform.DetachChildren();
        Destroy(gameObject);
    }
}
