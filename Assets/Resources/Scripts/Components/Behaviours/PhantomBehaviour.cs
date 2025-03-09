using System;
using System.Collections;
using UnityEngine;

public class MemoryBehaviour : ActivableBehaviour
{
    [Header("References")]
    [SerializeField] MeshRenderer renderer;

    public void Show(bool on)
    {
        Fade(on);
    }

    IEnumerator Fade(bool i)
    {
        Color rendererColor = renderer.material.color;
        //renderer.material.DOColor(new Color(rendererColor.r, rendererColor.g, rendererColor.b, Convert.ToInt32(i)), 1f);
        yield return new WaitForEndOfFrame();
    }
}
