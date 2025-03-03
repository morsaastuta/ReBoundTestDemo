using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceBehaviour : MonoBehaviour
{
    [SerializeField] List<EventBehaviour> events = new();
    [SerializeField] List<float> intervals = new();
    int index = 0;
    bool inProgress = false;
    EventBehaviour currentEvent;

    public void Begin()
    {
        index = 0;
        inProgress = true;
    }

    public void Continue()
    {
        // Clear and ready
        AudioManager.instance.StopVoice(true);
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
        if (inProgress && index == sessionIdx) Continue();
    }
}
