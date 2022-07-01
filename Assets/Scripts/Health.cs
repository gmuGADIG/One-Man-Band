using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHP;
    public int currentHP;
    public bool canDamage = true;

    public AudioClip[] hurtAudio;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }

    public void Damage(int dmg)
    {
        if(canDamage){
            currentHP -= dmg;
            bool isPlayer = (GetComponent<Player>()!= null);
            if(isPlayer){
                canDamage = false;
                Invoke("resetDamageFlag",.25f);
            }
            if (currentHP <= 0)
            {
                if (GetComponent<BaseEnemy>())
                {
                    GetComponent<BaseEnemy>().Die();
                }else if (GetComponent<Player>())
                {
                    GetComponent<Player>().Die();
                }
            }
            else if(!isPlayer)
            {
                GetComponent<AudioSource>().PlayOneShot(hurtAudio[Random.Range(0, hurtAudio.Length)]);
            }
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Note from Ben: This doesn't seem appropriate, as the Health component is on both enemies
        // and players?? Please let me know if I'm wrong.
        /*if (collision.CompareTag("Enemy"))
        {
            Damage(1);
        } */ 
    }

    void resetDamageFlag(){
        canDamage = true;
    }
}
