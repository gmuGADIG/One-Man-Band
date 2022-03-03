using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndScript : MonoBehaviour
{
    [Tooltip("The player character in this scene.")]
    public GameObject player;

    private PolygonCollider2D objCollider;
    private Collider2D playerCollider;
    // Start is called before the first frame update
    void Start()
    {
        objCollider = gameObject.GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (objCollider.IsTouching(playerCollider))
        {
            onPlayerTouch();
        }
    }

    private void onPlayerTouch()
    {

    }
}
