using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyAffiliation
{
    Red,
    Green,
    Blue
}

public class BaseEnemy : MonoBehaviour
{
    private int convertHealth;

    public int baseConvertHealth = 5;

    public EnemyAffiliation startingAffiliation = EnemyAffiliation.Red;

    private void Start()
    {
        convertHealth = baseConvertHealth;
        affiliation = startingAffiliation;
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

    // Converts the enemy to the given affiliation and resets its health.
    private void convertToAffiliation(EnemyAffiliation aff)
    {
        ChangeAffiliation(aff);
        convertHealth = baseConvertHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Notes noteScript = collision.gameObject.GetComponent<Notes>();

        // If we actually collided with a note...
        if(noteScript != null)
        {
            convertHealth -= noteScript.damage;
            if(convertHealth <= 0)
            {
                //convertToAffiliation(noteScript.affiliation);
            }
        }
    }
}
