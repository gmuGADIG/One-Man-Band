using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mix;
    [SerializeField] private List<AudioClip> tracks = new List<AudioClip>();

    [HideInInspector] public AudioSource[] sources;

    public int nextBranchIndex = 1;
    private float percentageBetweenTracks = 0;
    private GameManager manager;

    private GameObject progUI;

    private void Awake()
    {
        //This audio source will not be destoyed between scene loads
        MusicManager[] otherManagers = FindObjectsOfType<MusicManager>();
        if (otherManagers.Length > 1)
        {
            Destroy(gameObject);
        }
        sources = new AudioSource[tracks.Count]; //init array
        for (int i = 0; i < tracks.Count; i++) //assigns array and track properties
        {
            sources[i] = gameObject.AddComponent<AudioSource>();
            sources[i].clip = tracks[i];
            sources[i].outputAudioMixerGroup = mix;

            sources[i].loop = true;
            sources[i].Play();
            sources[i].volume = 0; 
        }
        FadeInTrack(0);
        
        manager = GetComponent<GameManager>();

        progUI = GameObject.Find("Progress Notes");
    }
    private void Start()
    {
        percentageBetweenTracks = ((float)manager.allNotes / sources.Length) / manager.allNotes;
        //Debug.Log(manager.allNotes + " " + sources.Length);
        //Debug.Log(percentageBetweenTracks + " PERCENT DIFFERENCE");
    }
    private void FixedUpdate()
    {
        //Debug.Log(manager.collectionPercent + " " + nextBranchIndex + " " + percentageBetweenTracks * nextBranchIndex);
        if ((percentageBetweenTracks * nextBranchIndex) <= manager.collectionPercent)
        {
            FadeInTrack(nextBranchIndex);
            nextBranchIndex++;

            Debug.Log("ELI - " + nextBranchIndex);

            progUI.GetComponent<ProgressUI>().moreProgress(nextBranchIndex);
        }
    }

    public void SetTrackVolume(int _track, float _volume = 1)
    {
        sources[_track].volume = _volume;
    }
    //basically couroutine in a function that fades
    public void FadeInTrack(int _track, float _targetVolume = 1, float overTime = 1)
    {
        IEnumerator Fade()
        {
            AudioSource targetTrack = sources[_track];
            float startingVolume = targetTrack.volume;
            float t = 0;
            
            while(targetTrack.volume < _targetVolume)
            {
                targetTrack.volume = Mathf.Lerp(startingVolume, _targetVolume, t);
                if (t < 1)
                {
                    t += Time.deltaTime / overTime;
                }
                yield return null;
            }
            yield return null;
        }
        StartCoroutine(Fade());
    }
    public void TurnOffMusic()
    {
        Destroy(gameObject);
    }

    public void SimplyMuteMusic()
    {
        for (int i = 0; i < tracks.Count; i++) //assigns array and track properties
        {
            sources[i].volume = 0;
        }
    }

    public void PickItBackUp()
    {
        switch (nextBranchIndex)
        {
            case 1:
                FadeInTrack(0);
                break;
            case 2:
                FadeInTrack(0);
                FadeInTrack(1);
                break;
            case 3:
                FadeInTrack(0);
                FadeInTrack(1);
                FadeInTrack(2);
                break;
            case 4:
                FadeInTrack(0);
                FadeInTrack(1);
                FadeInTrack(2);
                FadeInTrack(3);
                break;
            case 5:
                FadeInTrack(0);
                FadeInTrack(1);
                FadeInTrack(2);
                FadeInTrack(3);
                FadeInTrack(4);
                break;
        }
    }
}
