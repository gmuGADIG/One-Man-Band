using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void StartScene()
    {
        anim.SetTrigger("Start");
    }
    public void LoadScene(string scene) //AN EVENT SUBSCRIBER, dont call on button, call on animation function instead
    {
        SceneManager.LoadScene(scene);
    }
}
