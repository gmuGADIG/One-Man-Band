using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instruments : MonoBehaviour
{
    [Range(0.0f, 10.0f)]
    public float swap_speed = 5;
    [Range(0.0f, 5.0f)]
    public float AttackCooldown;
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

    private float FrameTimerAttack;
    private float FrameTimerSwitch;

    public Notes note;
    public GameObject player;
    public Camera main;

    public AudioSource playerSource;
    public AudioClip[] AttackAudio;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        FrameTimerSwitch += Time.deltaTime;
        FrameTimerAttack += Time.deltaTime;
        if (Input.GetKeyDown(attack) && AttackCooldown <= FrameTimerAttack) 
        {
            Vector3 cursorPosition = Input.mousePosition;
            //cursorPosition = Camera.main.ScreenToWorldPoint(cursorPosition) - transform.position; // - position makes it use local space meaning the player is the center instead of the global 0,0 coords
           

            cursorPosition = main.ScreenToWorldPoint(cursorPosition) - transform.position; // - position makes it use local space meaning the player is the center instead of the global 0,0 coords

			cursorPosition -= note.transform.position;
            Notes tempNote = Instantiate(note, player.transform);
            Debug.Log(cursorPosition);
            tempNote.setVelocity(cursorPosition, color);
            FrameTimerAttack = 0.0f;
            // playerSource.clip = AttackAudio[instrument_cycle];
            // playerSource.Play();
        }
        if (Input.GetKeyDown(instrument_swap) && swap_speed <= FrameTimerSwitch)
        {
            instrument_cycle++;
            if(instrument_cycle == 3)
            {
                instrument_cycle = 0;
            }
            if(instrument_cycle == Trumpet_cycle)
            {
                color = "Green";
            }
            else if(instrument_cycle == Flute_cycle)
            {
                color = "Red";
            }
            else if (instrument_cycle == Violin_cycle)
            {
                color = "Blue";
            }
            FrameTimerSwitch = 0.0f;
        }
        if (Input.GetKeyDown(Trumpet) && swap_speed <= FrameTimerSwitch)
        {
            instrument_cycle = Trumpet_cycle;
            color = "Green";
            FrameTimerSwitch = 0.0f;
        }
        if (Input.GetKeyDown(Flute) && swap_speed <= FrameTimerSwitch)
        {
            instrument_cycle = Flute_cycle;
            color = "Red";
            FrameTimerSwitch = 0.0f;
        }
        if (Input.GetKeyDown(Violin) && swap_speed <= FrameTimerSwitch)
        {
            instrument_cycle = Violin_cycle;
            color = "Blue";
            FrameTimerSwitch = 0.0f;
        }
    }
}
