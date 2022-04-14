using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEndGUIScript : MonoBehaviour
{
    [Tooltip("Name of next level scene")]
    public string nextLevel;
    private Text notesCollectedText;
    private int totalNotes;
    private int notesCollected;

    private GameObject songUnlockedText;
    private GameObject songLockedText;
    private GameObject notEnoughNotesText;
    // Start is called before the first frame update
    void Start()
    {
        notesCollected = ParentNote.notesCollected; //Avoids potential race conditions
        notesCollectedText = gameObject.transform.Find("NotesCollectedText").gameObject.GetComponent<Text>();
        setNotesCollectedText();

        songUnlockedText = transform.Find("SongUnlockedText").gameObject;
        songLockedText = transform.Find("SongLockedText").gameObject;
        notEnoughNotesText = transform.Find("NotEnoughNotesText").gameObject;

        gameObject.GetComponent<Canvas>().enabled = true;
        songUnlockedText.SetActive(false);
        songLockedText.SetActive(false);
        notEnoughNotesText.SetActive(false);
        setSongLockedText();
    }
    public void setTotalNotes(int totalNotes)
    {
        this.totalNotes = totalNotes;
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
            transform.Find("ContinueButton").gameObject.SetActive(false);
        }
    }
}
