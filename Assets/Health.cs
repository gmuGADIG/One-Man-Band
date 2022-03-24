using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHP;
    private int currentHP;
    
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }

    public void Damage(int dmg)
    {
        currentHP -= dmg;
        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
