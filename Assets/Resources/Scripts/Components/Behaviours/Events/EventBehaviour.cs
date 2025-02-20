using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventBehaviour : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioClip monologue;
    [SerializeField] AudioClip musicClip;
    [SerializeField] AudioSource musicSource;
    [SerializeField] List<AudioClip> soundClips = new();
    [SerializeField] List<AudioSource> soundSources = new();
    [SerializeField] List<AudioClip> voiceClips = new();
    [SerializeField] List<AudioSource> voiceSources = new();

    [Header("Subtitles")]
    [SerializeField] string content;

    [Header("Mechanisms")]
    [SerializeField] List<ActivableBehaviour> activableObjects = new();
    [SerializeField] List<bool> reversed = new();

    public void Play()
    {
        // Audio
        if (monologue != null) AudioManager.instance.PlayVoice(true, monologue);
        if (musicClip != null)
        {
            if (musicSource != null) AudioManager.instance.PlayMusic(true, musicClip, musicSource);
            else AudioManager.instance.PlayMusic(true, musicClip);
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
            if (source != null) AudioManager.instance.PlayVoice(true, clip, source);
            else AudioManager.instance.PlayVoice(true, clip);
        }

        // Subtitles
        if (EventManager.instance.subtitled) EventManager.instance.SetSubtitles(content);
        else EventManager.instance.ClearSubtitles();

        // Mechanisms
        foreach (ActivableBehaviour activableObject in activableObjects)
        {
            if (reversed[activableObjects.IndexOf(activableObject)]) activableObject.Deactivate();
            else activableObject.Activate();
        }
    }
}
