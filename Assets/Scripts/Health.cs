using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHP;
    public int currentHP;
    
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }

    public void Damage(int dmg)
    {
        currentHP -= dmg;

        // Comment out this for now. Probably shouldn't apply to the player.
        //if (currentHP <= 0)
        //{
        //    Destroy(gameObject);
        //}
    }

    // Update is called once per frame
    void Update()
    {
    }
}
