using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceBehaviour : ActivableBehaviour
{
    [SerializeField] List<EventBehaviour> events = new();
    bool breakpoint;
    int index = 0;
    EventBehaviour currentEvent;

    protected override void Start()
    {
        if (active) Begin();
    }

    public void Begin()
    {
        index = 0;
        Activate();
        Continue();
    }

    public void Continue()
    {
        // Clear and ready
        if (!events[index].keepVoice) AudioManager.instance.StopVoice(true);
        SubtitleManager.instance.ClearSubtitles();

        // Get event and play it
        currentEvent = events[index];
        currentEvent.Play();

        // Prepare next event
        index++;
        if (index >= events.Count) Deactivate();
        else StartCoroutine(AutoWait(index));
    }

    IEnumerator AutoWait(int sessionIdx)
    {
        yield return new WaitForSeconds(currentEvent.GetLength());

        while (currentEvent.breakpoint) yield return new WaitForSeconds(0.1f);

        if (active && index == sessionIdx) Continue();
    }
}
