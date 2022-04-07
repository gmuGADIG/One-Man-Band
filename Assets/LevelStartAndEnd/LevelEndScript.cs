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
		player = GameObject.FindGameObjectWithTag("Player");
        objCollider = gameObject.GetComponent<PolygonCollider2D>();
        playerCollider = player.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    /*void Update()
    {
        if (objCollider.IsTouching(playerCollider))
        {
            
        }
    }*/ 
	private void OnTriggerEnter2D(Collider2D collision) // we can use the built in trigger instead of checking every frame
	{
		if (collision.gameObject == player)
		{
			onPlayerTouch();
		}
	}
	private void onPlayerTouch()
    {
        stopAllEnemies();
        //Play an animation(?), call the end level UI
		//TODO: please make this load the next scene also
        
    }

    private void stopAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(false);
        }
        //Stop the enemy spawners too
        GameObject[] enemySpawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
        foreach (GameObject enemySpawner in enemySpawners)
        {
            enemySpawner.GetComponent<EnemySpawnerScript>().spawnCheck = false;
        }
    }
}
