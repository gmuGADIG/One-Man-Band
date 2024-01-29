using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapNote : MonoBehaviour
{
    public GameObject[] enemies;
    public AudioClip jacePoppingOff;
    public Transform[] spawnPoints;


    public GameManager gm;
    public GameObject player;
    public bool pickedUp = false;

    // Start is called before the first frame update
    void Start()
    {
        gm = (GameManager)GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        player = gm.GetPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.tag);

        if (collision.tag == "Player" && pickedUp == false)
        {
            pickedUp = true;

            GetComponent<AudioSource>().PlayOneShot(jacePoppingOff);

            foreach (Transform point in spawnPoints)
            {
                Instantiate(enemies[Random.Range(0, enemies.Length)], new Vector2(point.position.x, point.position.y), Quaternion.identity);
            }

            Invoke("destroyThis", 5.0f);
        }
    }

    private void destroyThis()
    {
        Destroy(gameObject);
    }
}
