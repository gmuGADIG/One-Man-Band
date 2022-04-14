using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBounce : MonoBehaviour
{
    public float speed = 5f;
    public float distance = .5f;
    Vector3 basePosition;
    // Start is called before the first frame update
    void Start()
    {
        basePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position =new Vector3(basePosition.x, basePosition.y + distance * Mathf.Sin(Time.time * speed), basePosition.z);
    }
}
