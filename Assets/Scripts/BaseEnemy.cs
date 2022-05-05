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

    public int convertHealth;
    public int baseConvertHealth = 5;
	
	public int Health;
    public int baseHealth = 5;
	protected GameObject Target;

    public bool damagedByRed = true;
    public bool damagedByBlue = true;
    public bool damagedByGreen = true;

    // Affiliation must be changed through ChangeAffiliation.
    // This makes it clear that the affiliation change may have additional side effects.
    public EnemyAffiliation affiliation { get; private set; }

    protected void Start()
    {
        convertHealth = baseConvertHealth;
        affiliation = EnemyAffiliation.AgainstPlayer;
		Target = GameObject.FindGameObjectWithTag("Player");
    }

    public void ChangeAffiliation(EnemyAffiliation newAffiliation)
    {
        EnemyAffiliation oldAffiliation = affiliation;

        affiliation = newAffiliation;
        OnAffiliationChanged(oldAffiliation, newAffiliation);
    }

    protected virtual void OnAffiliationChanged(EnemyAffiliation oldAffiliation, EnemyAffiliation newAffiliation)
    {
		
    }
	public virtual void Die()
	{
		Destroy(gameObject);
	}
	protected void Update()
	{
		
		if(affiliation == EnemyAffiliation.WithPlayer)
		{
			GameObject closest = null;
			foreach (BaseEnemy be in FindObjectsOfType<BaseEnemy>())
			{
                if(be == this) { continue; } //no targeting oneself
                float newDist = Vector3.Distance(be.transform.position, transform.position);                
                if ((!closest || (be.affiliation != affiliation && newDist < Vector3.Distance(closest.transform.position, transform.position))))
				{
					closest = be.gameObject;
				}
			}
			Target = closest;
		}
	}

	private void checkConvertNoteCollide(Collider2D collision)
    {
        // Already converted, don't need to check for conversion notes
        if (convertHealth <= 0) return;

        Notes noteScript = collision.gameObject.GetComponent<Notes>();

        // If we actually collided with a note...
        if (noteScript != null)
        {
            if (noteScript.red && !damagedByRed) return;
            if (noteScript.green && !damagedByGreen) return;
            if (noteScript.blue && !damagedByBlue) return;

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
