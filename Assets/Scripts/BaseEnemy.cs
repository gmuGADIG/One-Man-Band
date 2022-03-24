using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyAffiliation
{
    AgainstPlayer,
    WithPlayer
}

public class BaseEnemy : MonoBehaviour
{
    private int convertHealth;

    public int baseConvertHealth = 5;

    protected void Start()
    {
        convertHealth = baseConvertHealth;
        affiliation = EnemyAffiliation.AgainstPlayer;
    }

    // Affiliation must be changed through ChangeAffiliation.
    // This makes it clear that the affiliation change may have additional side effects.
    public EnemyAffiliation affiliation { get; private set; }
    public void ChangeAffiliation(EnemyAffiliation newAffiliation)
    {
        EnemyAffiliation oldAffiliation = affiliation;

        affiliation = newAffiliation;
        OnAffiliationChanged(oldAffiliation, newAffiliation);
    }

    protected virtual void OnAffiliationChanged(EnemyAffiliation oldAffiliation, EnemyAffiliation newAffiliation)
    {

    }

    private void checkConvertNoteCollide(Collider2D collision)
    {
        // Already converted, don't need to check for conversion notes
        if (convertHealth <= 0) return;

        Notes noteScript = collision.gameObject.GetComponent<Notes>();

        // If we actually collided with a note...
        if (noteScript != null)
        {
            convertHealth -= noteScript.damage;
            if (convertHealth <= 0)
            {
                ChangeAffiliation(EnemyAffiliation.WithPlayer);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        checkConvertNoteCollide(collision);
    }
}
