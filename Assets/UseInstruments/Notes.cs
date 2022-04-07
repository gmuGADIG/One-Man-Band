using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notes : MonoBehaviour
{

    private bool red, green, blue;
    //public EnemyAffiliation affiliation { get; private set; }
    [Range(0,100)]
    public int damage;
    [Range(0, 100)]
    public float moveSpeed;
    [Range(0, 1)]
    public float Scale;
    private Vector3 cursorPosition;
    private Vector2 forceVelocity = new Vector2(0.0f, 0.0f);
    //public GameObject note;


    public Sprite noteSprite;


    void Start()
    {
        /*note.*/transform.localScale = new Vector3(Scale, Scale, 1.0f);
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

    private void OnEnable()
    {
    }

    public void setVelocity(Vector2 vel, string color)
    {
        forceVelocity = vel;
        float angleZ = 0.0f;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; // - position makes it use local space meaning the player is the center instead of the global 0,0 coords
        angleZ = Mathf.Atan2(mousePos.x, mousePos.y) * -Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angleZ);
        if (color.Equals("Blue"))
        {
            red = false;
            blue = true;
            green = false;

            /*note.*/GetComponent<SpriteRenderer>().sprite = noteSprite;
            /*note.*/GetComponent<SpriteRenderer>().color = Color.blue;

        }
        else if (color.Equals("Red"))
        {
            red = true;
            blue = false;
            green = false;

            /*note.*/GetComponent<SpriteRenderer>().sprite = noteSprite;
            /*note.*/GetComponent<SpriteRenderer>().color = Color.red;

        }
        else if (color.Equals("Green"))
        {
            red = false;
            blue = false;
            green = true;

            /*note.*/GetComponent<SpriteRenderer>().sprite = noteSprite;
            /*note.*/GetComponent<SpriteRenderer>().color = Color.green;

        }
        /*note.*/tag = color + "Note";
    }

    void Move()
    {
        /*note.*/transform.position += (Vector3.Normalize(forceVelocity) * moveSpeed) * Time.deltaTime;
        /*note.*/transform.position = new Vector3(/*note.*/transform.position.x, /*note.*/transform.position.y, 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // Uhh... notes do not damage enemies. LOL!!!!!!
        // I've commented this out.
        // ... to give a little more info, the Health script is, AFAIK, the enemy health--
        // that is, once it's out of health, it will die.
        // The player Notes script should actually change the BaseEnemy.convertHealth value.
        // However, right now, the BaseEnemy script is already handling this behavior.

        //Health h  = collision.gameObject.GetComponent<Health>();
        //if (h) //if our object has a health script on it at all
        //{
        //     h.Damage(damage);
        //}

    }
}

