using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedMenuManager : MonoBehaviour
{
    private Animator anim;

    void Start(){
        anim = GetComponent<Animator>();
    }

    public void SwitchMenu(string animationTrigger) {
        anim.SetTrigger(animationTrigger);
    }
}
