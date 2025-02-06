using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Music tracks")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] List<AudioClip> musicTracks = new();

    [Header("Sound clips")]
    [SerializeField] List<AudioClip> soundClips = new();

    [Header("Voice lines")]
    [SerializeField] AudioSource voiceSource;
    [SerializeField] List<AudioClip> voiceLines = new();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else instance = this;
        DontDestroyOnLoad(gameObject);
    }
}