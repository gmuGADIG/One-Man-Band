using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    public GameObject[] spriteUi;
    //public GameObject currentHealthSprite;

    

    //public Canvas thisCanvas;

    public GameObject player;
    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = (GameManager)GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        player = gm.GetPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        switch (player.GetComponent<Health>().currentHP)
        {
            case 4:
                spriteUi[4].SetActive(false);
                spriteUi[0].SetActive(true);
                break;
            case 3:
                spriteUi[0].SetActive(false);
                spriteUi[1].SetActive(true);
                break;
            case 2:
                spriteUi[1].SetActive(false);
                spriteUi[2].SetActive(true);
                break;
            case 1:
                spriteUi[2].SetActive(false);
                spriteUi[3].SetActive(true);
                break;
            case 0:
                spriteUi[3].SetActive(false);
                spriteUi[4].SetActive(true);
                break;
        }
    }
}
