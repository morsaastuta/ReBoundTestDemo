using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ActivableBehaviour : MonoBehaviour // TO-DO: Create class inheritance with a new script ActivableBehaviour
{
    [SerializeField] protected List<ButtonBehaviour> assignedButtons;

    [Header("Audio")]
    [SerializeField] AudioClip activateClip;
    [SerializeField] AudioClip deactivateClip;
    AudioSource source;

    bool requirement = false;
    bool active = false;

    virtual protected void Start()
    {
        source = GetComponent<AudioSource>();
    }

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
        else if (!requirement && active) Deactivate();
    }

    virtual protected void Activate()
    {
        active = true;
        source.clip = activateClip;
        source.Play();
    }

    virtual protected void Deactivate()
    {
        active = false;
        source.clip = deactivateClip;
        source.Play();
    }
}
