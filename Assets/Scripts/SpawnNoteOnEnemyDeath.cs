using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNoteOnEnemyDeath : MonoBehaviour
{
    //Change these to types
    [SerializeField] private GameObject Note;
    [SerializeField] private List<Enemy> Enemies;

    void Start() 
    {
        //Change this to Notes disable
        Note.enabled = false;
    }
    private void OnEnable(){
        //Subscribe Cycle checks to each Enemy
    }
    private void OnDisable() {
        //Unsubscribe Cycle cehcks from each Enemy
    }
    private void CycleChecks() {
        float amountRemaining = 0;
        foreach (Enemy guy in Enemies) {
            if (!guy.isAlive) {
                amountRemaining++;
            }
        }
        if (amountRemaining == 0) {
            //Enable Note
        }
    }
}
