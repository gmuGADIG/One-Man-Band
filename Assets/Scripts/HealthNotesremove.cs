using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthNotesremove : MonoBehaviour
{
    public GameObject[] notePops;
    public GameObject noteToPop;
    public GameObject player;
    public GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = (GameManager)GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        player = gm.GetPlayer();
    }

    private void Update()
    {
        GetComponent<Slider>().value = player.GetComponent<Health>().currentHP;
    }
    // Update is called once per frame
    public void notePop()
    {
<<<<<<< HEAD
        Debug.Log("hdfsua");
        for (int i =0; i < notePops.Length; i++ )
        {
            if (i >= GetComponent<Slider>().value)
            {
                Debug.Log("Notes popping " + notePops[i].name);
                noteToPop = notePops[i];
                LeanTween.scale(notePops[i], new Vector3(0, 0, 0), 1.0f).setOnComplete(() => DestroyMe(noteToPop));
                break;
            }
        }
=======
        /*foreach (GameObject notePoping in notePops)
        {
            if(notePoping.active == false)
        }*/
>>>>>>> eb021f7b370c573a1fb0dd4ae47276320509bdde
    }

    public void DestroyMe(GameObject noteToPops)
    {
        noteToPops.SetActive(false);
    }
}
