using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notes : MonoBehaviour
{
    public float radiusAOE;
    public bool red, green, blue;
    public float moveSpeed;
    private Vector3 cursorPosition;
    private Vector2 forceVelocity = new Vector2(0.0f, 0.0f);
    public GameObject note;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        


        Move();
        if (Input.GetMouseButtonDown(0))
        {
            cursorPosition = Input.mousePosition;
            cursorPosition = Camera.main.ScreenToWorldPoint(cursorPosition);
            cursorPosition -= note.transform.position;
            setVelocity(cursorPosition, "Red");
        }
    }

    public void setVelocity(Vector2 vel, string color)
    {
        forceVelocity = vel;
        if (color.Equals("Blue"))
        {
            red = false;
            blue = true;
            green = false;
        }
        else if (color.Equals("Red"))
        {
            red = true;
            blue = false;
            green = false;
        }
        else if (color.Equals("Green"))
        {
            red = false;
            blue = false;
            green = true;
        }
    }
    
    void Move()
    {
        note.transform.position += (Vector3.Normalize(forceVelocity) * moveSpeed) * Time.deltaTime;
        note.transform.position = new Vector3(note.transform.position.x, note.transform.position.y, 0.0f);
    }
}
