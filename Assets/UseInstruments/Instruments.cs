using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instruments : MonoBehaviour
{
    [Range(0.0f, 10.0f)]
    public float swap_speed = 5;

    public KeyCode attack = KeyCode.E; 
    public Notes notes;

    public GameObject notes_sprite;


    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown == attack)
        {
            return;
        }
    }
}
