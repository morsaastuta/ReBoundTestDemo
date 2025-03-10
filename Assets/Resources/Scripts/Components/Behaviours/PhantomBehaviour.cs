using System;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;

public class PhantomBehaviour : MonoBehaviour
{
    [Header("Customization")]
    [SerializeField] List<Renderer> renderers;
    [SerializeField] float fadeWait = 2f;

    Vector3 destination;

    void Start()
    {
        Hide();
    }

    public void Hide()
    {
        foreach (Renderer renderer in renderers) Fade(renderer, false);
    }

    public void Pose(int idx)
    {
        Hide();
        Fade(renderers[idx], true);
    }

    void Fade(Renderer renderer, bool intro)
    {
        Color rendererColor = renderer.material.color;
        renderer.material.DOColor(new Color(rendererColor.r, rendererColor.g, rendererColor.b, Convert.ToInt32(intro)), fadeWait);
    }

    public void SequenceIn(int[] indexes)
    {
        StartCoroutine(Transit(indexes, true));
    }

    public void SequenceOut(int[] indexes)
    {
        StartCoroutine(Transit(indexes, false));
    }

    IEnumerator Transit(int[] indexes, bool intro)
    {
        foreach (int idx in indexes)
        {
            Fade(renderers[idx], true);

            yield return new WaitForSeconds(fadeWait);

            Hide();
        }
    }
}
