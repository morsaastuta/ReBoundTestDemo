using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Music tracks")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] List<AudioClip> musicClips = new();

    [Header("Sound clips")]
    [SerializeField] AudioSource soundSource;
    [SerializeField] List<AudioClip> soundClips = new();

    [Header("Voice lines")]
    [SerializeField] AudioSource voiceSource;
    [SerializeField] List<AudioClip> voiceClips = new();

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

    public void PlayMusic(bool on, int index)
    {
        if (on)
        {
            musicSource.clip = musicClips[index];
            musicSource.Play();
        }
        else musicSource.Stop();
    }

    public void PlayMusic(bool on, int index, AudioSource source)
    {
        if (on)
        {
            source.clip = musicClips[index];
            source.Play();
        }
        else source.Stop();
    }

    public void PlaySound(int index)
    {
        soundSource.clip = soundClips[index];
        soundSource.Play();
    }

    public void PlaySound(int index, AudioSource source)
    {
        source.clip = soundClips[index];
        source.Play();
    }

    public void PlayVoice(bool on, int index)
    {
        if (on)
        {
            voiceSource.clip = voiceClips[index];
            voiceSource.Play();
        }
        else voiceSource.Stop();
    }

    public void PlayVoice(bool on, int index, AudioSource source)
    {
        if (on)
        {
            source.clip = voiceClips[index];
            source.Play();
        }
        else source.Stop();
    }
}