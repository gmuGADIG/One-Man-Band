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
    public int Trumpet_cycle = 0;
    public int Flute_cycle = 1;
    public int Violin_cycle = 2;
    public KeyCode Trumpet = KeyCode.Keypad1;
    public KeyCode Flute = KeyCode.Keypad2;
    public KeyCode Violin = KeyCode.Keypad3;
    public string color = "Red";

    public Notes note;
    public GameObject player;

    public GameObject notes_sprite;

    public AudioSource playerSource;
    public AudioClip[] AttackAudio;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(attack)) 
        {
            Vector3 cursorPosition = Input.mousePosition;
            cursorPosition = Camera.main.ScreenToWorldPoint(cursorPosition);
            cursorPosition -= note.transform.position;
            Notes tempNote = Instantiate(note, player.transform);
            tempNote.setVelocity(cursorPosition, color);
            // playerSource.clip = AttackAudio[instrument_cycle];
            // playerSource.Play();
        }
        if (Input.GetKeyDown(instrument_swap))
        {
            instrument_cycle++;
            if(instrument_cycle == 3)
            {
                instrument_cycle = 0;
            }
            if(instrument_cycle == Trumpet_cycle)
            {
                color = "Green";
                Debug.Log("Trumpet ACTIVATED");
                
            }
            else if(instrument_cycle == Flute_cycle)
            {
                color = "Red";
                Debug.Log("Flute ACTIVATED");
            }
            else if (instrument_cycle == Violin_cycle)
            {
                color = "Blue";
                Debug.Log("Violin ACTIVATED");
            }
            
        }
        if (Input.GetKeyDown(Trumpet))
        {
            instrument_cycle = Trumpet_cycle;
            color = "Green";
            Debug.Log("Trumpet in use");
        }
        if (Input.GetKeyDown(Flute))
        {
            instrument_cycle = Flute_cycle;
            color = "Red";
            Debug.Log("Flute in use");
        }
        if (Input.GetKeyDown(Violin))
        {
            instrument_cycle = Violin_cycle;
            color = "Blue";
            Debug.Log("Violin in use");
        }
    }
}
