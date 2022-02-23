using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    //Sets the separate menu variables
    public Canvas TitleUI;
    public Canvas OptionsUI;

    void Start()
    {
        //Manually sets the two public menu variables
        TitleUI = GameObject.Find("TitleScreenCanvas").GetComponent<Canvas>();
        OptionsUI = GameObject.Find("OptionsCanvas").GetComponent<Canvas>();

        //Starts with the title screen enabled and the options screen false
        TitleUI.enabled = true;
        OptionsUI.enabled = false;
    }

    public void QuitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
        Debug.Log("QUIT");
    }

    //Function for entering the options menu
    public void OptionSelect()
    {
        TitleUI.enabled = false;
        OptionsUI.enabled = true;
    }

    //Function for exiting the option menu back to title screen
    public void OptionsBack()
    {
        TitleUI.enabled = true;
        OptionsUI.enabled = false;
    }

}
