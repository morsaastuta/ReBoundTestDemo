using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class SequenceBehaviour : MonoBehaviour
{
    [SerializeField] List<EventBehaviour> events = new();
    [SerializeField] List<float> intervals = new();
    int index = 0;
    bool on = false;

    public void Restart()
    {
        index = 0;
        on = true;
        Play();
    }

    public void Play()
    {
        if (on) events[index].Play();
    }

    public void Next()
    {
        index++;
        if (on)
        {
            on = true;
            Play();
        }
    }

    public void Stop()
    {
        if (on) events[index].Play();
    }
}
