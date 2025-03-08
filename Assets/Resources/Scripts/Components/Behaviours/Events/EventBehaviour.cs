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

    [Header("Mechanisms")]
    [SerializeField] List<ActivableBehaviour> activableObjects = new();
    [SerializeField] List<bool> reversed = new();

    [Header("Events")]
    [SerializeField] List<UnityEvent> events = new();

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
        if (EventManager.instance.subtitled) EventManager.instance.SetSubtitles(subtitles);
        else EventManager.instance.ClearSubtitles();

        // Mechanisms
        foreach (ActivableBehaviour activableObject in activableObjects)
        {
            if (reversed[activableObjects.IndexOf(activableObject)]) activableObject.Deactivate();
            else activableObject.Activate();
        }

        // Events
        foreach (UnityEvent function in events) function.Invoke();
    }

    public float GetLength()
    {
        float maxLength = fixedLength;

        if (monologue != null && monologue.length > maxLength) maxLength = monologue.length;
        foreach(AudioClip voiceClip in voiceClips) if (voiceClip != null && voiceClip.length > maxLength) maxLength = voiceClip.length;

        return maxLength + delay;
    }
}
