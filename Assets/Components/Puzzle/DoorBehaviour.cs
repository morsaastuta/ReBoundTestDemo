using DG.Tweening;
using UnityEngine;

public class DoorBehaviour : ActivableBehaviour // TO-DO: Create class inheritance with a new script ActivableBehaviour
{
    [Header("Rotation customization")]
    [SerializeField] float rotationAngles = 90;
    [SerializeField] bool clockwise = true;
    [SerializeField] float duration = 1;

    override protected void Activate()
    {
        base.Activate();

        if (!clockwise) transform.DOLocalRotate(transform.rotation.eulerAngles + new Vector3(0, -rotationAngles, 0), duration);
        else transform.DOLocalRotate(transform.rotation.eulerAngles + new Vector3(0, rotationAngles, 0), duration);
    }

    override protected void Deactivate()
    {
        base.Deactivate();

        if (!clockwise) transform.DOLocalRotate(transform.rotation.eulerAngles + new Vector3(0, rotationAngles, 0), duration);
        else transform.DOLocalRotate(transform.rotation.eulerAngles + new Vector3(0, -rotationAngles, 0), duration);
    }
}
