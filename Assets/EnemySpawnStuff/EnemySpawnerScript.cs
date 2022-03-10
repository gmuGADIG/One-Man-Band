using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    [Tooltip("Original enemy (or any object) for this spawner to spawn")]
    public GameObject enemyType;
    [Tooltip("Number of enemies spawned at a time, but may spawn less if spawning this many would produce more enemies in the level than the int Max Enemies")]
    public uint spawnSize;
    [Tooltip("Only spawns enemies if less than this number of enemies are alive in the level")]
    public uint maxEnemies;
    [Tooltip("Spawner will wait at least this many seconds between spawning each enemy")]
    public float spawnIntervalLowerBound;
    [Tooltip("Spawner will wait at most this many seconds between spawning each enemy")]
    public float spawnIntervalUpperBound;
    [Tooltip("Spawns enemies only while this is true/checked")]
    public bool spawnCheck;
    [Tooltip("The player object in this scene, the spawner only spawns enemies if the player is close enough to it")]
    public GameObject player;
    [Tooltip("Only spawns enemies if player is within this distance of spawner")]
    public float maxSpawningDistance;

    public static uint enemiesAliveInLevel; //Should be set to 0 by the scene when the level starts, 
                                            //decremented by enemies as they despawn

    private float nextSpawn = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
		player = GameObject.FindGameObjectWithTag("Player");
        incrementNextSpawn();
        //Below if statement sets the count of alive enemies to 0 if it's currently the start of the scene
        if (Time.frameCount <= 1)
        {
            enemiesAliveInLevel = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawn)
        {
            bool playerIsClose = Vector3.Distance(player.transform.position, gameObject.transform.position) < maxSpawningDistance;
            //spawnCheck and playerIsClose are separated from nextSpawn to allow random increments to continue
            //(prevents spawner(s) from immediately spawning once spawnCheck is set to true/player gets close enough)
            if (spawnCheck && playerIsClose)
            {
                uint amountToSpawn = spawnSize; //Number of enemies to spawn this time
                if (spawnSize + enemiesAliveInLevel > maxEnemies) //If spawning spawnSize enemies isn't allowed by maxEnemies
                {
                    amountToSpawn = maxEnemies - enemiesAliveInLevel; //Only spawn enough enemies to reach maxEnemies
                }
                for (uint i = 0; i < amountToSpawn; ++i)
                {
                    Object.Instantiate(enemyType, gameObject.transform.position, gameObject.transform.rotation);
                    enemiesAliveInLevel++;

                }
            }
            incrementNextSpawn();
        }
    }

    private void incrementNextSpawn()
    {
        nextSpawn += Random.Range(spawnIntervalLowerBound, spawnIntervalUpperBound);
    }
}
