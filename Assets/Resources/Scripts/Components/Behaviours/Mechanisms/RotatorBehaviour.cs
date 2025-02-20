using DG.Tweening;
using UnityEngine;

public class RotatorBehaviour : ActivableBehaviour
{
    [Header("Customization")]
    [SerializeField] bool bridge = false;
    [SerializeField] float rotationAngles = 90;
    [SerializeField] bool clockwise = true;
    [SerializeField] float duration = 1;

    override public void Activate()
    {
        base.Activate();

        if (!bridge)
        {
            if (!clockwise) transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, -rotationAngles, 0), duration);
            else transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, rotationAngles, 0), duration);
        }
        else
        {
            if (!clockwise) transform.DORotate(transform.rotation.eulerAngles + new Vector3(-rotationAngles, 0, 0), duration);
            else transform.DORotate(transform.rotation.eulerAngles + new Vector3(rotationAngles, 0, 0), duration);
        }
    }

    override public void Deactivate()
    {
        base.Deactivate();

        if (!bridge)
        {
            if (!clockwise) transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, rotationAngles, 0), duration);
            else transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, -rotationAngles, 0), duration);
        }
        else
        {
            if (!clockwise) transform.DORotate(transform.rotation.eulerAngles + new Vector3(rotationAngles, 0, 0), duration);
            else transform.DORotate(transform.rotation.eulerAngles + new Vector3(-rotationAngles, 0, 0), duration);
        }
    }
}
