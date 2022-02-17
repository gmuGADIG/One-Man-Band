using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    [Tooltip("Original enemy (or any object) for this spawner to spawn")]
    public Object enemyType;
    [Tooltip("Number of enemies spawned at a time")]
    public uint spawnSize;
    [Tooltip("Spawner will wait at least this many seconds between spawning each enemy")]
    public float spawnIntervalLowerBound;
    [Tooltip("Spawner will wait at most this many seconds between spawning each enemy")]
    public float spawnIntervalUpperBound;
    [Tooltip("Spawns enemies only while this is true/checked")]
    public bool spawnCheck;

    private float nextSpawn = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        incrementNextSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawn)
        {
            //spawnCheck is separated from nextSpawn to allow random increments to continue
            //(prevents spawner(s) from immediately spawning once spawnCheck is set to true)
            if (spawnCheck)
            {
                for (uint i = 0; i < spawnSize; ++i)
                {
                    Object.Instantiate(enemyType, gameObject.transform.position, gameObject.transform.rotation);
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
