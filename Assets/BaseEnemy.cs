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
    private EnemyAffiliation affiliation;

    public void changeAffiliation(EnemyAffiliation newAffiliation)
    {
        EnemyAffiliation oldAffiliation = affiliation;

        affiliation = newAffiliation;
        onAffiliationChanged(oldAffiliation, newAffiliation);
    }

    protected virtual void onAffiliationChanged(EnemyAffiliation oldAffiliation, EnemyAffiliation newAffiliation)
    {

    }
}
