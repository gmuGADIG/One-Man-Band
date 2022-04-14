using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    [Tooltip("If this collides with something that does not have a collision soundit will play a sound unless the object has one of these tages")] 
    [SerializeField] private List<string> ignoredTags;
    [SerializeField] private AudioClip[] sounds;
    [SerializeField] [Range(1, 100)] private int priority = 50;

    private AudioSource source;
    private void Start()
    {
        if (!TryGetComponent(out source))
        {
            source = gameObject.AddComponent<AudioSource>();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CollisionSound othersSound;
        if (collision.gameObject.TryGetComponent(out othersSound))
        {
            if (othersSound.priority > priority)
            {
                source.PlayOneShot(UsefulFunctions.ReturnRandomElement(sounds));
            }
            else if (othersSound.priority == priority)
            {
                Debug.LogWarning("A collision of equal priority occured, no sound played.");
            }
        }
        else if (!ignoredTags.Contains(collision.gameObject.tag))
        {
            source.PlayOneShot(UsefulFunctions.ReturnRandomElement(sounds));
        }
    }
}
