using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventBehaviour : MonoBehaviour
{
    [Header("Duration")]
    [SerializeField] float fixedLength = 0;
    [SerializeField] float delay = 0.5f;
    [SerializeField] public bool breakpoint = false;

    [Header("Voice")]
    [SerializeField] public bool keepVoice;
    [SerializeField] AudioClip monologue;
    [SerializeField] List<AudioClip> voiceClips = new();
    [SerializeField] List<AudioSource> voiceSources = new();
    [TextArea][SerializeField] string subtitles;

    [Header("BGM/SFX")]
    [SerializeField] AudioClip musicClip;
    [SerializeField] AudioSource musicSource;
    [SerializeField] bool stopMusic;
    [SerializeField] List<AudioClip> soundClips = new();
    [SerializeField] List<AudioSource> soundSources = new();

    [Header("Functions")]
    [SerializeField] UnityEvent functions = new();

    public void Play()
    {
        // Audio
        if (monologue != null) AudioManager.instance.PlayVoice(monologue);

        if (stopMusic) AudioManager.instance.StopMusic();
        else if (musicClip != null)
        {
            if (musicSource != null) AudioManager.instance.PlayMusic(musicClip, musicSource);
            else AudioManager.instance.PlayMusic(musicClip);
        }

        foreach (AudioClip clip in soundClips)
        {
            AudioSource source = soundSources[soundClips.IndexOf(clip)];
            if (source != null) AudioManager.instance.PlaySound(clip, source);
            else AudioManager.instance.PlaySound(clip);
        }

        foreach (AudioClip clip in voiceClips)
        {
            AudioSource source = voiceSources[voiceClips.IndexOf(clip)];
            if (source != null) AudioManager.instance.PlayVoice(clip, source);
            else AudioManager.instance.PlayVoice(clip);
        }

        // Subtitles
        if (SubtitleManager.instance.subtitled) SubtitleManager.instance.SetSubtitles(subtitles);
        else SubtitleManager.instance.ClearSubtitles();

        // Functions
        functions.Invoke();
    }

    public float GetLength()
    {
        float length = 0;

        if (fixedLength <= 0)
        {
            if (monologue != null && monologue.length > length) length = monologue.length;
            foreach (AudioClip voiceClip in voiceClips) if (voiceClip != null && voiceClip.length > length) length = voiceClip.length;
        }
        else length = fixedLength;

        return length + delay;
    }
}
