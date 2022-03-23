using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovementDummy : MonoBehaviour
{
    private float x;
    private float y;
    // Start is called before the first frame update
    void Start()
    {
        x = Random.Range(-0.01f, 0.01f);
        y = Random.Range(-0.01f, 0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += new Vector3(x,y);
    }
}
