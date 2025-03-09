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
    [SerializeField] GameObject button;
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