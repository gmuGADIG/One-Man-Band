using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndScript : MonoBehaviour
{
    [Tooltip("Name of scene to load upon player")]
    public string nextScene;
    
    private GameObject player;
    private Player playerMoveScript;
    private Instruments playerAttackScript;
    private PolygonCollider2D objCollider;
    private Collider2D playerCollider;
    private GameObject levelEndGUI;
    private LevelEndGUIScript GUIscript;
    private int totalNotes;
    // Start is called before the first frame update
    void Start()
    {
		player = GameObject.FindGameObjectWithTag("Player");
        playerMoveScript = player.GetComponent<Player>();
        playerAttackScript = player.GetComponent<Instruments>();
        objCollider = gameObject.GetComponent<PolygonCollider2D>();
        playerCollider = player.GetComponent<Collider2D>();
        levelEndGUI = GameObject.Find("LevelEndGUI");
        GUIscript = levelEndGUI.GetComponent<LevelEndGUIScript>();
        if (levelEndGUI == null)
        {
            Debug.Log("Please add a LevelEndGUI to the scene!");
        }
        levelEndGUI.SetActive(false);
        GUIscript.enabled = true;
        totalNotes = countNotesInScene();
        GUIscript.setTotalNotes(totalNotes);
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

    private int countNotesInScene()
    {
        return GameObject.FindGameObjectsWithTag("Note").Length +
               GameObject.FindGameObjectsWithTag("TimedNotes").Length +
               GameObject.FindGameObjectsWithTag("Daddy").Length;
    }
}
