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
    public static GameManager gm;

    [HideInInspector] public float collectionPercent = 0;

    public void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
        else
        {
            Destroy(gameObject);
        }
        player = GameObject.FindGameObjectWithTag("Player");
        allNotes = FindObjectsOfType<ParentNote>().Length;
        Debug.Log("Notes at start: " + allNotes);
    }

    public void changeSong()
    {
        //Debug.Log("Notes at start: " + allNotes);
        //Debug.Log("Notes collected: " + notesCollected);
        //Debug.Log("Notes % = " + ((allNotes-notesCollected)/notesCollected));
    }
    private void Update()
    {
        //Debug.Log(notesCollected + " " + allNotes);
        collectionPercent = (float)notesCollected / allNotes;
        //Debug.Log(collectionPercent);
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
