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
    }
}
