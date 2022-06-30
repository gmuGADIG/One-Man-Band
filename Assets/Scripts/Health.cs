using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHP;
    public int currentHP;

    public AudioClip[] hurtAudio;

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
			if (GetComponent<BaseEnemy>())
			{
				GetComponent<BaseEnemy>().Die();
			}else if (GetComponent<Player>())
			{
				GetComponent<Player>().Die();
			}
		}
        else
        {
            GetComponent<AudioSource>().PlayOneShot(hurtAudio[Random.Range(0, hurtAudio.Length)]);
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
}
