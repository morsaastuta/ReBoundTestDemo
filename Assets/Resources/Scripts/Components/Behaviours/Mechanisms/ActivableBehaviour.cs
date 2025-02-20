using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ActivableBehaviour : MonoBehaviour // TO-DO: Create class inheritance with a new script ActivableBehaviour
{
    [SerializeField] protected List<ButtonBehaviour> assignedButtons;
    [SerializeField] protected bool permanent = false;

    [Header("Audio")]
    [SerializeField] AudioClip activateClip;
    [SerializeField] AudioClip deactivateClip;
    [SerializeField] AudioSource source;

    [Header("Sequence")]
    [SerializeField] SequenceBehaviour sequence;

    bool requirement = false;
    bool active = false;

    virtual protected void FixedUpdate()
    {
        // Requirement is true by default
        requirement = true;

        // Set requirement to false if any button is not pressed
        foreach(ButtonBehaviour button in assignedButtons)
        {
            if (!button.pressed)
            {
                requirement = false;
                break;
            }
        }

        // Activation / Deactivation
        if (requirement && !active) Activate();
        else if (!permanent && !requirement && active) Deactivate();
    }

    virtual public void Activate()
    {
        active = true;
        source.clip = activateClip;
        source.Play();
        if (sequence != null) sequence.Play();
    }

    virtual public void Deactivate()
    {
        active = false;
        source.clip = deactivateClip;
        source.Play();
    }
}
