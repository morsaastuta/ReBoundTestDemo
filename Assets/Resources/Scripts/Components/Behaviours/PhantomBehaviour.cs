using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class PhantomBehaviour : ActivableBehaviour
{
    [Header("Customization (Phantom)")]
    [SerializeField] List<AnimationClip> animations = new();

    [Header("References (Phantom)")]
    [SerializeField] Renderer renderer;
    [SerializeField] Animation animator;

    public override void Activate()
    {
        base.Activate();
        Fade(true);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        Fade(false);
    }

    public void Animate(int animIdx)
    {
        animator.clip = animations[animIdx];
        animator.Play();
    }

    void Fade(bool on)
    {
        Color rendererColor = renderer.material.color;
        renderer.material.DOColor(new Color(rendererColor.r, rendererColor.g, rendererColor.b, Convert.ToInt32(on)), 2f);
    }
}
