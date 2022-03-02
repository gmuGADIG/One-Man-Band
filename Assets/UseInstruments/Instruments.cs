using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instruments : MonoBehaviour
{
    [Range(0.0f, 10.0f)]
    public float swap_speed = 5;

    public KeyCode attack = KeyCode.E;
    public KeyCode instrument_swap = KeyCode.Tab;
    public int instrument_cycle = 0;
    public int Trumpet_cycle = 1;
    public int Flute_cycle = 2;
    public int Violin_cycle = 3;
    public KeyCode Trumpet = KeyCode.Keypad1;
    public KeyCode Flute = KeyCode.Keypad2;
    public KeyCode Violin = KeyCode.Keypad3;
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
        if(Input.GetKeyDown(attack)) 
			/* Eyo so GetButtonDown is actually a function that takes in a string (which you can bind to a specific key in the player editor tab) and returns a bool 
			 * since you are trying to compare it to a keycode (attack) you would actually wanna use GetKeyDown() (which takes in a keycode and returns a bool) 
			 * so instead of "Input.GetButtonDown == attack" you would do Input.GetKeyDown(attack).
			 * - David :)
			 * PS if you wanna know more about this stuff feel free to ask me or check out the unity documentation, its really good!
			 */
		{
            return;
        }
        if (Input.GetKeyDown(instrument_swap))
        {
            if(instrument_cycle == Trumpet_cycle)
            {
                Debug.Log("Trumpet ACTIVATED");
                instrument_cycle += 1;
            }
            else if(instrument_cycle == Flute_cycle)
            {
                Debug.Log("Flute ACTIVATED");
                instrument_cycle += 1;
            }
            else if (instrument_cycle == Violin_cycle)
            {
                Debug.Log("Violin ACTIVATED");
                instrument_cycle += 1;
            }
            else
            {
                Debug.Log("MELE ACTIVATED");
                instrument_cycle = 1;
            }
            
        }
        if (Input.GetKeyDown(Trumpet))
        {
            instrument_cycle = Trumpet_cycle + 1;
            Debug.Log("Trumpet in use");
        }
        if (Input.GetKeyDown(Flute))
        {
            instrument_cycle = Flute_cycle + 1;
            Debug.Log("Flute in use");
        }
        if (Input.GetKeyDown(Violin))
        {
            instrument_cycle = Violin_cycle + 1;
            Debug.Log("Violin in use");
        }
    }
}
