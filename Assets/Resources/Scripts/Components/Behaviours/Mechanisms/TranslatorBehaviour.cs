using DG.Tweening;
using UnityEngine;

public class TranslatorBehaviour : ActivableBehaviour
{    
    [Header("Customization")]
    [SerializeField] Vector3 direction = new();
    [SerializeField] float duration = 1;

    Vector3 ogPos = new();

    protected void Start()
    {
        ogPos = transform.position;
    }

    override public void Activate()
    {
        base.Activate();

        transform.DOMove(ogPos + direction, duration);
    }

    override public void Deactivate()
    {
        base.Deactivate();

        transform.DOMove(ogPos, duration);
    }
}
