using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This can be used on any GUI, on said gui and your custom animations n such and on the buttons call their triggers

public class AnimatedMenuManager : MonoBehaviour
{
    private Animator anim;
    private SceneTransitioner transition;

    void Start(){
        anim = GetComponent<Animator>();
        transition = FindObjectOfType<SceneTransitioner>();
    }

    public void SwitchMenu(string animationTrigger) {
        anim.SetTrigger(animationTrigger);
    }
    public void ChangeScenesSmoothly(string nextLoadScene)
    {
        transition.StartScene(nextLoadScene);
    }
    public void QuitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
