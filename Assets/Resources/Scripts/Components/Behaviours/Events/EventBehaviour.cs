using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventBehaviour : MonoBehaviour
{
    [SerializeField] float fixedLength;

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
        if (monologue != null) AudioManager.instance.PlayVoice(monologue);
        if (musicClip != null)
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
        if (EventManager.instance.subtitled) EventManager.instance.SetSubtitles(content);
        else EventManager.instance.ClearSubtitles();

        // Mechanisms
        foreach (ActivableBehaviour activableObject in activableObjects)
        {
            if (reversed[activableObjects.IndexOf(activableObject)]) activableObject.Deactivate();
            else activableObject.Activate();
        }
    }

    public float GetLength()
    {
        float maxLength = fixedLength;

        if (monologue != null && monologue.length > maxLength) maxLength = monologue.length;
        foreach(AudioClip voiceClip in voiceClips) if (voiceClip != null && voiceClip.length > maxLength) maxLength = voiceClip.length;

        return maxLength + 0.5f;
    }
}
