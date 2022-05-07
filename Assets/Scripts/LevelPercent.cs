using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPercent : MonoBehaviour
{
    public GameManager gm;
    public Slider Sliders;
    // Start is called before the first frame update
    void Start()
    {
        Sliders = GetComponent<Slider>();
        gm = (GameManager)GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        Sliders.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Sliders.value = gm.collectionPercent;
    }
}
