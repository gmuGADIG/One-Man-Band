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
        TitleUI.gameObject.SetActive(true);
        OptionsUI.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
		Application.Quit();
        Debug.Log("QUIT");
    }

    //Function for entering the options menu
    public void OptionSelect()
    {
        TitleUI.gameObject.SetActive(false);
        OptionsUI.gameObject.SetActive(true);
    }

    //Function for exiting the option menu back to title screen
    public void OptionsBack()
    {
        TitleUI.gameObject.SetActive(true);
        OptionsUI.gameObject.SetActive(false);
    }

}
