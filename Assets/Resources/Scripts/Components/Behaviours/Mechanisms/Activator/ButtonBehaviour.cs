using DG.Tweening;
using UnityEngine;
using static Glossary;

public class ButtonBehaviour : ActivatorBehaviour
{
    [Header("Customization (Button)")]
    [SerializeField] AudioClip pressClip;
    [SerializeField] AudioClip unpressClip;

    [Header("References (Button)")]
    [SerializeField] GameObject button;
    [SerializeField] Transform onPos;
    [SerializeField] Transform offPos;
    [SerializeField] AudioSource source;

    void OnCollisionEnter(Collision collision)
    {
        if (!active && LayerMask.LayerToName(collision.collider.gameObject.layer).Equals(GetLayer(Layer.Ball))) Activate(true);
    }

    protected override void Activate(bool on)
    {
        base.Activate(on);

        if (on)
        {
            source.clip = pressClip;
            source.Play();
            button.transform.DOMove(onPos.transform.position, .5f);
        }
        else
        {
            source.clip = unpressClip;
            source.Play();
            button.transform.DOMove(offPos.transform.position, .5f);
        }
    }
}