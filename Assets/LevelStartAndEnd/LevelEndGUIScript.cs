using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEndGUIScript : MonoBehaviour
{
    [Tooltip("Name of next level scene")]
    public string nextLevel;
    private Text notesCollectedText;
    // Start is called before the first frame update
    void Start()
    {
        notesCollectedText = gameObject.transform.Find("NotesCollectedText").gameObject.GetComponent<Text>();
        setNotesCollectedText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setNotesCollectedText()
    {
        string result = string.Format("{0} / {1} notes collected", ParentNote.notesCollected, "???"); //TODO: Set ??? to the total number of collectable notes in the level
        notesCollectedText.text = result;
    }
}
