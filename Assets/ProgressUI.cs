using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressUI : MonoBehaviour
{
    public GameManager gm;

    public GameObject[] progressUI;
    public GameObject yayParticles;

    public GameObject player;

    // Start is called before the first frame update Yay Party Time
    void Start()
    {
        
        gm = (GameManager)GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
		player = gm.GetPlayer();
		yayParticles = GameObject.Find("Yay Party Time");
    }

    // Update is called once per frame
    void Update()
    {
        //switch (gm.collectionPercent)
        //{
        //    case 
        //}
    }

    public void moreProgress(int progressUIIndex)
    {
        switch (progressUIIndex)
        {
            case 2:
                progressUI[0].SetActive(false);
                progressUI[1].SetActive(true);
                break;
            case 3:
                progressUI[1].SetActive(false);
                progressUI[2].SetActive(true);
                break;
            case 4:
                progressUI[2].SetActive(false);
                progressUI[3].SetActive(true);
                break;
            case 5:
                progressUI[3].SetActive(true);
                progressUI[4].SetActive(true);
                yayParticles.SetActive(true);
                break;
        }

        Debug.Log(progressUIIndex);
    }
}
