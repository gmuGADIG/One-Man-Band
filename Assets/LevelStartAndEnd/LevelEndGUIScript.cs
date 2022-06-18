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

    private GameObject panel;
    private GameObject songUnlockedText;
    private GameObject songLockedText;
    private GameObject notEnoughNotesText;
    private GameObject continueButton;
    private GameObject managerObject;
    private GameManager managerScript;
    private GameObject levelLoader;
    private SceneTransitioner sceneTransitioner;
    private LevelEndScript levelEndScript;
    private Animator animator;

    private void Awake()
    {
        OnEnable();
    }

    // Start is called before the first frame update

    /*void Start()
    {
        levelLoader = GameObject.Find("LevelLoader");
        sceneTransitioner = levelLoader.GetComponentInChildren<SceneTransitioner>();
        levelEndScript = GameObject.Find("LevelEnd").GetComponent<LevelEndScript>();
        
        panel = gameObject.transform.Find("Panel").gameObject;
        continueButton = panel.transform.Find("ContinueButton").gameObject;
        notesCollectedText = panel.transform.Find("NotesCollectedText").gameObject.GetComponent<Text>();
        managerObject = GameObject.FindGameObjectWithTag("GameController");
        managerScript = managerObject.GetComponent<GameManager>();
        setNotesCollectedText();

        
        songUnlockedText = panel.transform.Find("SongUnlockedText").gameObject;
        songLockedText = panel.transform.Find("SongLockedText").gameObject;
        notEnoughNotesText = panel.transform.Find("NotEnoughNotesText").gameObject;
        
        songUnlockedText.SetActive(false);
        songLockedText.SetActive(false);
        notEnoughNotesText.SetActive(false);
        setSongLockedText();
    } */

    private void OnEnable()
    {
        levelLoader = GameObject.Find("LevelLoader");
        sceneTransitioner = levelLoader.GetComponentInChildren<SceneTransitioner>();
        levelEndScript = GameObject.Find("LevelEnd").GetComponent<LevelEndScript>();

        panel = gameObject.transform.Find("Panel").gameObject;
        continueButton = panel.transform.Find("ContinueButton").gameObject;
        notesCollectedText = panel.transform.Find("NotesCollectedText").gameObject.GetComponent<Text>();
        managerObject = GameObject.FindGameObjectWithTag("GameController");
        managerScript = managerObject.GetComponent<GameManager>();
        setNotesCollectedText();


        songUnlockedText = panel.transform.Find("SongUnlockedText").gameObject;
        songLockedText = panel.transform.Find("SongLockedText").gameObject;
        notEnoughNotesText = panel.transform.Find("NotEnoughNotesText").gameObject;

        songUnlockedText.SetActive(false);
        songLockedText.SetActive(false);
        notEnoughNotesText.SetActive(false);
        setSongLockedText();

        animator = gameObject.GetComponent<Animator>();
        animator.Play("ZoomIn");
    }

    void setNotesCollectedText()
    {
        string result = string.Format("{0} / {1} notes collected", managerScript.GetNotesCollected(), managerScript.allNotes);
        notesCollectedText.text = result;
    }

    void setSongLockedText()
    {
        double percentNotesCollected = (double)managerScript.GetNotesCollected() / managerScript.allNotes;
        continueButton.SetActive(true);
        if (managerScript.GetNotesCollected() >= managerScript.allNotes)
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
            int neededNotes = Mathf.CeilToInt(0.7f * managerScript.allNotes);
            notEnoughNotesText.GetComponent<Text>().text = string.Format("You must collect at least {0} notes to continue.", neededNotes);
            notEnoughNotesText.SetActive(true);
            continueButton.SetActive(false);
        }
    }

    public void continueButtonClicked()
    {
        sceneTransitioner.StartScene(nextLevel);
    }

    public void retryButtonClicked()
    {
        animator.Play("ZoomOut");
    }

    public void zoomOutFinished() //Called by an event in the ZoomOut animation
    {
        levelEndScript.resumeGame();
    }

    public void menuButtonClicked()
    {
        sceneTransitioner.StartScene("TitleScreen");
    }
}
