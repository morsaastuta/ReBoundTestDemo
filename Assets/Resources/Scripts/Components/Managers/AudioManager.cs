using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    enum Channel
    {
        Music, Sound, Voice
    }

    [Header("Customization")]
    [SerializeField] public AudioClip defaultBGM;

    [Header("Music references")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] List<AudioClip> musicClips = new();

    [Header("Sound references")]
    [SerializeField] AudioSource soundSource;
    [SerializeField] List<AudioClip> soundClips = new();

    [Header("Voice references")]
    [SerializeField] AudioSource voiceSource;
    [SerializeField] List<AudioClip> voiceClips = new();

    // Memory
    [HideInInspector] public float globalVolume = 1f;
    [HideInInspector] public float musicVolume = 0.5f;
    [HideInInspector] public float soundVolume = 0.5f;
    [HideInInspector] public float voiceVolume = 0.5f;
    List<AudioSource> musicSources = new();
    List<AudioSource> voiceSources = new();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            instance.defaultBGM = defaultBGM;
            Destroy(gameObject);
            return;
        }
        else instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (defaultBGM != null) PlayMusic(defaultBGM);
        else StopMusic();
    }

    void Play(AudioClip clip, float volume, AudioSource source, Channel channel)
    {
        source.clip = clip;

        if (channel == Channel.Music) StartCoroutine(Fade(source, volume * globalVolume));
        else source.volume = volume * globalVolume;

        source.Play();
    }

    void Stop(AudioSource source)
    {
        StartCoroutine(Fade(source, 0));
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

    IEnumerator Fade(AudioSource source, float targetVolume)
    {
        float updateDelay = 0.01f;
        float fadeEffect = 0.1f;

        if (targetVolume > 0)
        {
            while (source.volume < targetVolume)
            {
                yield return new WaitForSeconds(updateDelay);
                source.volume += updateDelay * fadeEffect;
            }
        }
        else
        {
            while (source.volume > 0)
            {
                yield return new WaitForSeconds(updateDelay);
                source.volume -= updateDelay * fadeEffect;
            }
            source.Stop();
        }
    }

    #region Play & Stop sub-methods

    public void PlayMusic(AudioClip clip)
    {
        Play(clip, musicVolume, musicSource, Channel.Music);
    }

    public void PlayMusic(AudioClip clip, AudioSource source)
    {
        if (!musicSources.Contains(source)) musicSources.Add(source);

        Play(clip, musicVolume, source, Channel.Music);
    }

    public void PlaySound(AudioClip clip)
    {
        Play(clip, soundVolume, soundSource, Channel.Sound);
    }

    public void PlaySound(AudioClip clip, AudioSource source)
    {
        Play(clip, soundVolume, source, Channel.Sound);
    }

    public void PlayVoice(AudioClip clip)
    {
        StopVoice(true);
        Play(clip, voiceVolume, voiceSource, Channel.Voice);
    }

    public void PlayVoice(AudioClip clip, AudioSource source)
    {
        if (!voiceSources.Contains(source)) voiceSources.Add(source);

        StopVoice(true);
        Play(clip, voiceVolume, source, Channel.Voice);
    }

    public void StopMusic()
    {
        Stop(musicSource);
    }

    public void StopVoice(bool all)
    {
        if (!all) Stop(voiceSource);
        else
        {
            Stop(voiceSource);
            foreach (AudioSource source in voiceSources) if (source != null) Stop(source);
        }
    }

    public void StopSource(AudioSource source)
    {
        Stop(source);
    }

    #endregion
}