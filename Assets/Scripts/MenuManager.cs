using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public Canvas TitleUI;
    public Canvas OptionsUI;

    void Start()
    {
        TitleUI.enabled = true;
        OptionsUI.enabled = false;
    }

    public void QuitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
        Debug.Log("QUIT");
    }

    public void OptionSelect()
    {
        TitleUI.enabled = false;
        OptionsUI.enabled = true;
    }

    public void OptionsBack()
    {
        TitleUI.enabled = true;
        OptionsUI.enabled = false;
    }

}
