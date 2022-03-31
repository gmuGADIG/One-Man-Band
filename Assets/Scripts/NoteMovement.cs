using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMovement : MonoBehaviour
{
    public float speed;
    private float RotateSpeed = 5f;
    public float Radius = 2f;

    public enum Direction { line, circle }
    public Direction dir = new Direction();
    private Vector2 centre;
    private float angle;
    // Start is called before the first frame update
    void Start()
    {
        centre = transform.position;
       // rCount = Random.Range(1, 3);
    }

    // Update is called once per frame
    void Update()
    {
        

        if (dir == Direction.line)
        {
            float y = Mathf.PingPong(Time.time * speed, 1) * 6 - 3;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, y, 0);
        }
        else if (dir == Direction.circle)
        {

            angle += RotateSpeed * Time.deltaTime;

            var offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * Radius;
            transform.position = centre + offset;
        }
    }
}
