using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    private Animator anim;
    private string nextScene;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void StartScene(string _nextScene)
    {
        nextScene = _nextScene;
        anim.SetTrigger("Start");
    }
    public void LoadScene() //AN EVENT SUBSCRIBER, dont call on button, call on animation function instead
    {
        SceneManager.LoadScene(nextScene);
    }
}
