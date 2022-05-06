using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthNotesremove : MonoBehaviour
{
    public GameObject[] notePops;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void notePop()
    {
        /*foreach (GameObject notePoping in notePops)
        {
            if(notePoping.active == false)
        }*/
    }

    public void DestroyMe(GameObject noteToPop)
    {
        Destroy(noteToPop);
    }
}
