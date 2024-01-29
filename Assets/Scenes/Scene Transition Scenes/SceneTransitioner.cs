using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    private Animator anim;
    string nextScene;

    [SerializeField] private bool playAnimationOnStart = true;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        if (playAnimationOnStart)
            anim.SetTrigger("PlayEntry");
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
