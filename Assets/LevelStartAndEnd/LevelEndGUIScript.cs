using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelEndGUIScript : MonoBehaviour
{
    [Tooltip("Name of next level scene")]
    public string nextLevel;
    private Text notesCollectedText;
    private int totalNotes;
    private int notesCollected;

    private GameObject panel;
    private GameObject songUnlockedText;
    private GameObject songLockedText;
    private GameObject notEnoughNotesText;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update

    void Start()
    {
        panel = gameObject.transform.Find("Panel").gameObject;
        notesCollected = ParentNote.notesCollected; //Avoids potential race conditions
        notesCollectedText = panel.transform.Find("NotesCollectedText").gameObject.GetComponent<Text>();
        setNotesCollectedText();

        
        songUnlockedText = panel.transform.Find("SongUnlockedText").gameObject;
        songLockedText = panel.transform.Find("SongLockedText").gameObject;
        notEnoughNotesText = panel.transform.Find("NotEnoughNotesText").gameObject;
        
        songUnlockedText.SetActive(false);
        songLockedText.SetActive(false);
        notEnoughNotesText.SetActive(false);
        setSongLockedText();
    }

    public void setTotalNotes(int num)
    {
        totalNotes = num;
    }

    void setNotesCollectedText()
    {
        string result = string.Format("{0} / {1} notes collected", notesCollected, totalNotes);
        notesCollectedText.text = result;
    }

    void setSongLockedText()
    {
        double percentNotesCollected = (double)notesCollected / totalNotes;
        
        if (notesCollected == totalNotes)
        {
            //All notes were collected
            songUnlockedText.SetActive(true);
        }
        else if (percentNotesCollected > 0.7)
        {
            //Enough notes were collected to continue
            songLockedText.SetActive(true);
        }
        else
        {
            //Not enough notes were collected to continue
            int neededNotes = Mathf.CeilToInt(0.7f * totalNotes);
            notEnoughNotesText.GetComponent<Text>().text = string.Format("You must collect at least {0} notes to continue.", neededNotes);
            notEnoughNotesText.SetActive(true);
            panel.transform.Find("ContinueButton").gameObject.SetActive(false);
        }
    }

    public void continueButtonClicked()
    {
        ParentNote.notesCollected = 0;
        SceneManager.LoadScene(nextLevel);
    }

    public void retryButtonClicked()
    {
        ParentNote.notesCollected = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void menuButtonClicked()
    {
        ParentNote.notesCollected = 0;
        SceneManager.LoadScene("TitleScreen");
    }
}
