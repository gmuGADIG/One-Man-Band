using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] public int notesCollected = 0;
    public int allNotes = 0;
    public int note_Percent = 0;
    [SerializeField] private int enemysInScene = 0;
    private List<BaseEnemy> enemies = new List<BaseEnemy>();
    public MusicManager musicManager;

    public void Start()
    {
        allNotes = FindObjectsOfType<ParentNote>().Length;
    }

    public void changeSong()
    {
        Debug.Log("Notes at start: " + allNotes);
        Debug.Log("Notes collected: " + notesCollected);
        Debug.Log("Notes % = " + ((allNotes-notesCollected)/notesCollected)* 100);
    }

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
