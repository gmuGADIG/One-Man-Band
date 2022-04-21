using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] public int notesCollected = 0;
    [SerializeField] private int enemysInScene = 0;
    private List<BaseEnemy> enemies = new List<BaseEnemy>();

    public GameObject GetPlayer()
    {
        return player;
    }
    public int GetNotesCollected()
    {
        return notesCollected;
    }
    public void AddEnemyIntoSceneRegistry(BaseEnemy enemy)
    {
        enemies.Add(enemy);
        enemysInScene = enemies.Count;
    }
}
