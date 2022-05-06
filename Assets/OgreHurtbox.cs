using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreHurtbox : MonoBehaviour
{
    bool hurt;
    int damage;
    GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        damage = 1;
    }

    private void OnEnable()
    {
        hurt = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTarget(GameObject t)
    {
        target = t;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!hurt & collision.gameObject == target) {
            target.GetComponent<Health>().Damage(damage);
            hurt = true;
        }
        
    }
}
