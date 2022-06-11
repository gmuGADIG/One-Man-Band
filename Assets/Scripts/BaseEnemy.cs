using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyAffiliation
{

    AgainstPlayer = -1,
    Red = 0,
    Green = 1,
    Blue = 2,
    WithPlayer

}

public class BaseEnemy : MonoBehaviour
{

    public int convertHealth;
    public int baseConvertHealth = 5;
	
	public int Health;
    public int baseHealth = 5;
    [SerializeField]
	protected GameObject Target;
    [SerializeField]
    protected BaseEnemy enemyTarget;

    public bool damagedByRed = true;
    public bool damagedByBlue = true;
    public bool damagedByGreen = true;

    public GameObject isRed;
    public GameObject isGreen;
    public GameObject isBlue;

    // Affiliation must be changed through ChangeAffiliation.
    // This makes it clear that the affiliation change may have additional side effects.
    public EnemyAffiliation affiliation;

    protected void Start()
    {
        convertHealth = baseConvertHealth;
        affiliation = EnemyAffiliation.AgainstPlayer;
		Target = GameObject.FindGameObjectWithTag("Player");
    }

    public void ChangeAffiliation(EnemyAffiliation newAffiliation)
    {
        EnemyAffiliation oldAffiliation = affiliation;
        Debug.Log("Affiliation" + newAffiliation);
        affiliation = newAffiliation;
        OnAffiliationChanged(oldAffiliation, newAffiliation);
    }

    protected virtual void OnAffiliationChanged(EnemyAffiliation oldAffiliation, EnemyAffiliation newAffiliation)
    {
        convertHealth = baseConvertHealth;
        Target = FindTarget();
    }
	public virtual void Die()
	{
		Destroy(gameObject);
	}
	protected void Update()
	{
		if(Target == null || (enemyTarget != null && (int)enemyTarget.affiliation != ((int)affiliation + 1) % 3)){
            Target = FindTarget();
        }
	}

    protected GameObject FindTarget(){
        Debug.Log("Finding Target:"+ ((int)affiliation + 1) % 3);
        GameObject closest = null;
        foreach (BaseEnemy be in FindObjectsOfType<BaseEnemy>())
        {
            Debug.Log(be.affiliation);
            if(be == this) { continue; } //no targeting oneself
            if((int)be.affiliation == ((int)affiliation + 1) % 3){
                float newDist = Vector3.Distance(be.transform.position, transform.position);                
                if ((!closest || (newDist < Vector3.Distance(closest.transform.position, transform.position))))
                {
                    closest = be.gameObject;
                }
            }

        }
        Debug.Log(closest);
        if(closest == null){
            enemyTarget = null;
            closest = GameObject.FindGameObjectWithTag("Player");;
		}
		else
		{
            enemyTarget = closest.GetComponent<BaseEnemy>();
		}
        return closest;
    }

	protected void checkConvertNoteCollide(Collider2D collision)
    {
        // Already converted, don't need to check for conversion notes
        if (convertHealth <= 0) return;

        Notes noteScript = collision.gameObject.GetComponent<Notes>();

        // If we actually collided with a note...
        if (noteScript != null)
        {
            if (noteScript.red && affiliation == EnemyAffiliation.Red) return;
            if (noteScript.green && affiliation == EnemyAffiliation.Green) return;
            if (noteScript.blue && affiliation == EnemyAffiliation.Blue) return;

            convertHealth -= noteScript.damage;
            if (convertHealth <= 0)
            {
                Debug.Log("I'm dead LMAO");
                if(noteScript.red){
                    ChangeAffiliation(EnemyAffiliation.Red);
                    Debug.Log("red");
                    isRed.SetActive(true);
                }
                else if(noteScript.green){
                    ChangeAffiliation(EnemyAffiliation.Green);
                    Debug.Log("green");
                    isGreen.SetActive(true);
                }
                else if(noteScript.blue){
                    ChangeAffiliation(EnemyAffiliation.Blue);
                    Debug.Log("blue");
                    isBlue.SetActive(true);
                }
                else
                {
                    ChangeAffiliation(EnemyAffiliation.WithPlayer);
                }
                Debug.Log("DEAD");
                
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        checkConvertNoteCollide(collision);
    }
}
