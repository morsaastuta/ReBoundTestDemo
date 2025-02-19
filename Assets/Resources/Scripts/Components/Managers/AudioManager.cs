using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    [Header("Subtitles")]
    [SerializeField] TextMeshProUGUI subtitles;
    bool subtitled = true;

    // Memory
    float globalVolume = 1f;
    float musicVolume = 0.5f;
    float soundVolume = 0.5f;
    float voiceVolume = 0.5f;
    List<AudioSource> musicSources = new();
    List<AudioSource> voiceSources = new();

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

    void Play(AudioClip clip, float volume, AudioSource source)
    {
        source.clip = clip;
        source.volume = volume * globalVolume;
        source.Play();
    }

    void Stop(AudioSource source)
    {
        source.Stop();
    }

    public void SetVolume(float g, float m, float s, float v)
    {
        globalVolume = g;
        musicVolume = m;
        soundVolume = s;
        voiceVolume = v;

        UpdateSources();
    }

    void UpdateSources()
    {
        musicSource.volume = musicVolume;
        soundSource.volume = soundVolume;
        voiceSource.volume = voiceVolume;
        foreach (AudioSource source in musicSources) if (source != null) source.volume = musicVolume;
        foreach (AudioSource source in voiceSources) if (source != null) source.volume = voiceVolume;
    }

    #region Play & Stop sub-methods

    public void PlayMusic(bool on, AudioClip clip)
    {
        if (on) Play(clip, musicVolume, musicSource);
        else Stop(musicSource);
    }

    public void PlayMusic(bool on, AudioClip clip, AudioSource source)
    {
        if (!musicSources.Contains(source)) musicSources.Add(source);

        if (on) Play(clip, musicVolume, source);
        else Stop(source);
    }

    public void PlaySound(AudioClip clip, AudioSource source)
    {
        Play(clip, soundVolume, source);
    }

    public void PlaySound(AudioClip clip)
    {
        Play(clip, soundVolume, soundSource);
    }

    public void PlayVoice(bool on, AudioClip clip)
    {
        if (on) Play(clip, voiceVolume, voiceSource);
        else Stop(voiceSource);
    }

    public void PlayVoice(bool on, AudioClip clip, AudioSource source)
    {
        if (!voiceSources.Contains(source)) voiceSources.Add(source);

        if (on) Play(clip, voiceVolume, source);
        else Stop(source);
    }

    #endregion
}