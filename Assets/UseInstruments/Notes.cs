using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notes : MonoBehaviour
{
    private float radiusAOE;
    private bool red, green, blue;
    [Range(0,100)]
    public int damage;
    [Range(0 ,100)]
    public float moveSpeed;
    private Vector3 cursorPosition;
    private Vector2 forceVelocity = new Vector2(0.0f, 0.0f);
    public GameObject note;


    public Sprite noteSprite;


    void Start()
    {
        note.GetComponent<CircleCollider2D>().radius = radiusAOE;
    }

    // Update is called once per frame
    void Update()
    {
        


        Move();
        /*
        if (Input.GetMouseButtonDown(0))
        {
            cursorPosition = Input.mousePosition;
            cursorPosition = Camera.main.ScreenToWorldPoint(cursorPosition);
            cursorPosition -= note.transform.position;
            setVelocity(cursorPosition, "Red");
        }
        */
    }

    public void setVelocity(Vector2 vel, string color)
    {
        forceVelocity = vel;
        note.transform.LookAt(Input.mousePosition, Vector3.up);
        if (color.Equals("Blue"))
        {
            red = false;
            blue = true;
            green = false;
            note.GetComponent<SpriteRenderer>().sprite = noteSprite;
            note.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else if (color.Equals("Red"))
        {
            red = true;
            blue = false;
            green = false;
            note.GetComponent<SpriteRenderer>().sprite = noteSprite;
            note.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if (color.Equals("Green"))
        {
            red = false;
            blue = false;
            green = true;
            note.GetComponent<SpriteRenderer>().sprite = noteSprite;
            note.GetComponent<SpriteRenderer>().color = Color.green;
        }
        note.tag = color + "Note";
    }
    
    void Move()
    {
        note.transform.position += (Vector3.Normalize(forceVelocity) * moveSpeed) * Time.deltaTime;
        note.transform.position = new Vector3(note.transform.position.x, note.transform.position.y, 0.0f);
    }
}
