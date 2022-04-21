using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> tracks = new List<AudioClip>();

    [HideInInspector] public AudioSource[] sources;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        sources = new AudioSource[tracks.Count];
        for (int i = 0; i < tracks.Count; i++)
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
    public void TurnOffMusic()
    {
        Destroy(gameObject);
    }
}
