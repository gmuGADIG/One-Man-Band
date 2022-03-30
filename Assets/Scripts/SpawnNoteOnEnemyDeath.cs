using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNoteOnEnemyDeath : MonoBehaviour
{
    //Change these to types
    [SerializeField] private GameObject Note;
    [SerializeField] private List<BaseEnemy> Enemies;

    void Start() 
    {
        //Change this to Notes disable
        Note.SetActive(false);
    }
    private void OnEnable(){
        //Subscribe Cycle checks to each Enemy
    }
    private void OnDisable() {
        //Unsubscribe Cycle cehcks from each Enemy
    }
    private void CycleChecks() {
        float amountRemaining = 0;
        foreach (BaseEnemy guy in Enemies) {
            if (guy.convertHealth > 0 && guy.Health > 0) {
                amountRemaining++;
            }
        }
        if (amountRemaining == 0) {
            //Enable Note
        }
    }
}
