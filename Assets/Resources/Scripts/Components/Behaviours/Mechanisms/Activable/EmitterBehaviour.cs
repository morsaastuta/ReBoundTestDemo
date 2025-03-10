using DG.Tweening;
using UnityEngine;

public class EmitterBehaviour : ActivableBehaviour
{
    [Header("Customization (Emitter)")]
    [SerializeField] GameObject particles;

    override public void Activate()
    {
        base.Activate();
        particles = Instantiate(particles, transform);
    }

    override public void Deactivate()
    {
        base.Deactivate();
        Destroy(particles);
    }
}
