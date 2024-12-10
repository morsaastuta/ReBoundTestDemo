using DG.Tweening;
using UnityEngine;

public class MovableObjectBehaviour : ActivableBehaviour
{    
    [Header("Customization")]
    [SerializeField] Transform target;
    [SerializeField] Vector3 newPos = Vector3.zero;
    [SerializeField] float duration = 1;

    Vector3 ogPos = Vector3.zero;

    void Start()
    {
        ogPos = transform.position;
        if (target != null) newPos = target.position;
    }

    override protected void Activate()
    {
        base.Activate();

        transform.DOMove(newPos, duration);
    }

    override protected void Deactivate()
    {
        base.Deactivate();

        transform.DOMove(ogPos, duration);
    }
}
