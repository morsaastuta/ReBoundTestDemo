using DG.Tweening;
using UnityEngine;
using static Glossary;

public class TriggerBehaviour : ActivatorBehaviour
{
    [Header("Customization (Trigger)")]
    [SerializeField] AudioClip triggerClip;
    [SerializeField] AudioClip resetClip;

    [Header("References (Trigger)")]
    [SerializeField] GameObject button;
    [SerializeField] Transform onPos;
    [SerializeField] Transform offPos;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Animator animator;

    void OnCollisionEnter(Collision collision)
    {
        if (!active && LayerMask.LayerToName(collision.collider.gameObject.layer).Equals(GetLayer(Layer.Ball))) Activate(true);
    }

    protected override void Activate(bool on)
    {
        base.Activate(on);

        if (on)
        {
            if (animator != null) animator.SetBool("on", true);
            audioSource.clip = triggerClip;
            audioSource.Play();
            if (onPos != null) button.transform.DOMove(onPos.transform.position, .5f);
        }
        else
        {
            if (animator != null) animator.SetBool("on", false);
            audioSource.clip = resetClip;
            audioSource.Play();
            if (offPos != null) button.transform.DOMove(offPos.transform.position, .5f);
        }
    }

    public override bool Check()
    {
        return active;
    }
}