using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> tracks = new List<AudioClip>();

    [HideInInspector] public AudioSource[] sources;


    private void Awake()
    {
        //This audio source will not be destoyed between scene loads
        DontDestroyOnLoad(gameObject);
        sources = new AudioSource[tracks.Count]; //init array
        for (int i = 0; i < tracks.Count; i++) //assigns array and track properties
        {
            sources[i] = gameObject.AddComponent<AudioSource>();
            sources[i].clip = tracks[i];

            sources[i].loop = true;
            sources[i].Play();
            sources[i].volume = 0;
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
}
