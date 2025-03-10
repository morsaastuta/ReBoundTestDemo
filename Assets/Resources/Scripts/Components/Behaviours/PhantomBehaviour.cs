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
    [SerializeField] GameObject animationRoot;

    public override void Activate()
    {
        base.Activate();
        StartCoroutine(Fade(true));
    }

    public override void Deactivate()
    {
        base.Deactivate();
        StartCoroutine(Fade(false));
    }

    public void Animate(int animIdx)
    {
        animations[animIdx].SampleAnimation(animationRoot, animations[animIdx].length);
    }

    IEnumerator Fade(bool on)
    {
        Color rendererColor = renderer.material.color;
        renderer.material.DOColor(new Color(rendererColor.r, rendererColor.g, rendererColor.b, Convert.ToInt32(on)), 2f);

        if (on) renderer.enabled = true;
        yield return new WaitForSeconds(2f);
        if (!on) renderer.enabled = false;
    }
}
