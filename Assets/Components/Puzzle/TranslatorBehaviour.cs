using DG.Tweening;
using UnityEngine;

public class TranslatorBehaviour : ActivableBehaviour
{    
    [Header("Customization")]
    [SerializeField] Vector3 direction = new();
    [SerializeField] float duration = 1;

    Vector3 ogPos = new();

    override protected void Start()
    {
        base.Start();

        ogPos = transform.position;
    }

    override protected void Activate()
    {
        base.Activate();

        transform.DOMove(ogPos + direction, duration);
    }

    override protected void Deactivate()
    {
        base.Deactivate();

        transform.DOMove(ogPos, duration);
    }
}
