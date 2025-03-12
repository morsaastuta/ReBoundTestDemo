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

    public void SequenceIn(string indexes)
    {
        List<int> idxs = new();
        foreach (string idx in indexes.Split(' ')) idxs.Add(Convert.ToInt32(idx));
        StartCoroutine(Transit(idxs, true));
    }

    public void SequenceOut(string indexes)
    {
        List<int> idxs = new();
        foreach (string idx in indexes.Split(' ')) idxs.Add(Convert.ToInt32(idx));
        StartCoroutine(Transit(idxs, false));
    }
    
    IEnumerator Transit(List<int> indexes, bool intro)
    {
        int remainingIndexes = indexes.Count;

        foreach (int idx in indexes)
        {
            remainingIndexes--;

            Pose(idx);

            yield return new WaitForSeconds(fadeWait);

            if (!intro && remainingIndexes == 0) Hide();
        }
    }
}
