using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndScript : MonoBehaviour
{
    [Tooltip("Name of scene to load upon player")]
    public string nextScene;
    [Tooltip("Don't touch this please, leave this as the LevelEndGUI prefab")]
    public GameObject levelEndGUI;
    
    private GameObject player;
    private Player playerMoveScript;
    private Instruments playerAttackScript;
    private PolygonCollider2D objCollider;
    private Collider2D playerCollider;
    private LevelEndGUIScript GUIscript;
    private float playerPrevVelocity;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMoveScript = player.GetComponent<Player>();
        playerAttackScript = player.GetComponent<Instruments>();
        objCollider = gameObject.GetComponent<PolygonCollider2D>();
        playerCollider = player.GetComponent<Collider2D>();
        levelEndGUI = Instantiate(levelEndGUI);
        GUIscript = levelEndGUI.GetComponent<LevelEndGUIScript>();
        levelEndGUI.SetActive(false);
        GUIscript.enabled = true;
        GUIscript.nextLevel = nextScene;
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
        levelEndGUI.SetActive(true);
        playerPrevVelocity = playerMoveScript.maxVelocity;
        playerMoveScript.maxVelocity = 0;
        playerAttackScript.enabled = false;
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

    public void resumeGame()
    {
        //Resume the enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(true);
        }
        //Resume the enemy spawners too
        GameObject[] enemySpawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
        foreach (GameObject enemySpawner in enemySpawners)
        {
            enemySpawner.GetComponent<EnemySpawnerScript>().spawnCheck = true;
        }

        levelEndGUI.SetActive(false);
        playerMoveScript.maxVelocity = playerPrevVelocity;
        playerAttackScript.enabled = true;
    }
}
