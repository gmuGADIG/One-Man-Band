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

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Damage(1);
        }  
    }
}
