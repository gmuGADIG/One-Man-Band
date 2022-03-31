using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseChamp : MonoBehaviour
{
    public GameObject pauseScreen;
    public int counter = 0;

    void Start() {
        pauseScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            pauseScreen.SetActive(true);
            counter += 1;
            Pause();
        }
        else if (counter == 2)
        {
            counter = 0;
            pauseScreen.SetActive(false);
            Resume();
        }
    }

    void Pause (){
        Time.timeScale = 0;
    }
    
    void Resume (){
        Time.timeScale = 1;
    }

}
