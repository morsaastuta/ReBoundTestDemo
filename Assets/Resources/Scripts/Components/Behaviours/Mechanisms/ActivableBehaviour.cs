using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivableBehaviour : MonoBehaviour // TO-DO: Create class inheritance with a new script ActivableBehaviour
{
    [Header("Customization (Activable)")]
    [SerializeField] protected List<ActivatorBehaviour> activatorsAND;
    [SerializeField] protected List<ActivatorBehaviour> activatorsOR;
    [SerializeField] protected bool permanent = false;
    [SerializeField] AudioClip activateClip;
    [SerializeField] AudioClip deactivateClip;

    [Header("References (Activable)")]
    [SerializeField] AudioSource source;

    bool requirement = false;
    public bool active = false;

    protected virtual void FixedUpdate()
    {
        foreach (ActivatorBehaviour activator in activatorsOR)
        {
            if (activator.active)
            {
                requirement = true;
                break;
            }
        }
       
        foreach (ActivatorBehaviour activator in activatorsAND)
        {
            if (!activator.active)
            {
                requirement = false;
                break;
            }
        }

        if (activatorsAND.Count > 0 || activatorsOR.Count > 0)
        {
            // Automatic (de)activation
            if (requirement && !active) Activate();
            else if (!permanent && !requirement && active) Deactivate();
        }
    }

    public virtual void Activate()
    {
        active = true;

        if (activateClip != null)
        {
            source.clip = activateClip;
            source.Play();
        }
    }

    public virtual void Deactivate()
    {
        active = false;

        if (deactivateClip != null)
        {
            source.clip = deactivateClip;
            source.Play();
        }
    }
}
