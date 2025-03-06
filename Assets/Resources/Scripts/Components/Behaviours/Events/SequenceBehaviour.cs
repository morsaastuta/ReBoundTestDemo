using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceBehaviour : MonoBehaviour
{
    [SerializeField] List<EventBehaviour> events = new();
    [SerializeField] List<float> intervals = new();
    bool breakpoint;
    int index = 0;
    bool inProgress = false;
    EventBehaviour currentEvent;

    public void Begin()
    {
        index = 0;
        inProgress = true;
        Continue();
    }

    public void Continue()
    {
        // Clear and ready
        if (!events[index].keepVoice) AudioManager.instance.StopVoice(true);
        EventManager.instance.ClearSubtitles();

        // Get event and play it
        currentEvent = events[index];
        currentEvent.Play();

        // Prepare next event
        index++;
        if (index >= events.Count) End();
        else StartCoroutine(AutoWait(index));
    }

    public void End()
    {
        inProgress = false;
    }

    IEnumerator AutoWait(int sessionIdx)
    {
        yield return new WaitForSeconds(currentEvent.GetLength());

        while (currentEvent.breakpoint) yield return new WaitForSeconds(0.1f);

        if (inProgress && index == sessionIdx) Continue();
    }
}
