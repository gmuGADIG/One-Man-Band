using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentIsntrument : MonoBehaviour
{
    public Sprite[] Instrument;
    public GameObject player;
    public GameManager gm;
    public Image Im;
    // Start is called before the first frame update
    void Start()
    {
        Im = GetComponent<Image>();
        gm = (GameManager)GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        player = gm.GetPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        Im.sprite = Instrument[player.GetComponent<Instruments>().instrument_cycle];
    }
}
