using DG.Tweening;
using UnityEngine;
using static Glossary;

public class TriggerBehaviour : ActivatorBehaviour
{
    [Header("Customization (Trigger)")]
    [SerializeField] AudioClip triggerClip;
    [SerializeField] AudioClip resetClip;
    [SerializeField] bool onBreak = false;

    [Header("References (Trigger)")]
    [SerializeField] GameObject core;
    [SerializeField] Transform onPos;
    [SerializeField] Transform offPos;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Animator animator;

    void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.collider.gameObject;

        if (!active && LayerMask.LayerToName(go.layer).Equals(GetLayer(Layer.Ball)))
        {
            Ball ball = go.GetComponent<BallBehaviour>().ball;
             
            if (!onBreak || (onBreak && ball.reboundCount == ball.reboundLimit)) Activate(true);
        }
    }

    public override void Activate(bool on)
    {
        base.Activate(on);

        if (on)
        {
            if (triggerClip != null)
            {
                audioSource.clip = triggerClip;
                audioSource.Play();
            }

            if (animator != null) animator.SetBool("on", true);

            if (onPos != null) core.transform.DOMove(onPos.transform.position, .5f);
        }
        else
        {
            if (resetClip != null)
            {
                audioSource.clip = resetClip;
                audioSource.Play();
            }

            if (animator != null) animator.SetBool("on", false);

            if (offPos != null) core.transform.DOMove(offPos.transform.position, .5f);
        }
    }

    public override bool Check()
    {
        return active;
    }
}