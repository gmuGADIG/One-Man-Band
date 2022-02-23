using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notes : MonoBehaviour
{
    public float radiusAOE;
    public bool red, green, blue;
    public float moveSpeed;
    private Vector3 cursorPosition;
    public GameObject note;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    
    void Move()
    {
        cursorPosition = Input.mousePosition;
        cursorPosition = Camera.main.ScreenToWorldPoint(cursorPosition);
        note.transform.position = ((cursorPosition) / moveSpeed);
        note.transform.position = new Vector3(note.transform.position.x, note.transform.position.y, 0.0f);
    }
}
