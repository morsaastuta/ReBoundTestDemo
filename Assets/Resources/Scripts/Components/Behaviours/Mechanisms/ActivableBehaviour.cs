using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivableBehaviour : MonoBehaviour // TO-DO: Create class inheritance with a new script ActivableBehaviour
{
    [Header("Customization (Activable)")]
    [SerializeField] protected List<ActivatorBehaviour> activators;
    [SerializeField] protected bool permanent = false;
    [SerializeField] AudioClip activateClip;
    [SerializeField] AudioClip deactivateClip;

    [Header("References (Activable)")]
    [SerializeField] AudioSource source;

    bool requirement = false;
    public bool active = false;

    protected virtual void FixedUpdate()
    {
        if (activators.Count > 0)
        {
            // Requirement is true by default
            requirement = true;
       
            // Set requirement to false if any button is not pressed
            foreach (ActivatorBehaviour activator in activators)
            {
                if (!activator.active)
                {
                    requirement = false;
                    break;
                }
            }

            // Activation / Deactivation
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
