using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
