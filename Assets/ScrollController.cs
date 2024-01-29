using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollController : MonoBehaviour
{

    public Animator anim;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            anim.speed = 25;
        }else if (Input.GetKeyUp(KeyCode.Space)){
            anim.speed = 1;
        }
    }
}
