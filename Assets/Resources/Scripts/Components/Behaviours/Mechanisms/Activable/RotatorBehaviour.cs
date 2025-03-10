using DG.Tweening;
using System;
using UnityEngine;

public class RotatorBehaviour : ActivableBehaviour
{
    enum RotateDirection
    {
        X, Y, Z
    }

    [Header("Customization (Rotator)")]
    [SerializeField] float rotationAngles = 90;
    [SerializeField] bool positive = true;
    [SerializeField] float duration = 1;
    [SerializeField] RotateDirection rotateDirection = RotateDirection.Y;

    public override void Activate()
    {
        base.Activate();

        switch (rotateDirection)
        {
            case RotateDirection.X: RotateX(rotationAngles * Convert.ToInt32(positive)); break;
            case RotateDirection.Y: RotateY(rotationAngles * Convert.ToInt32(positive)); break;
            case RotateDirection.Z: RotateZ(rotationAngles * Convert.ToInt32(positive)); break;
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();

        switch (rotateDirection)
        {
            case RotateDirection.X: RotateX(-rotationAngles * Convert.ToInt32(positive)); break;
            case RotateDirection.Y: RotateY(-rotationAngles * Convert.ToInt32(positive)); break;
            case RotateDirection.Z: RotateZ(-rotationAngles * Convert.ToInt32(positive)); break;
        }
    }

    public void RotateX(float amount)
    {
        transform.DORotate(transform.rotation.eulerAngles + new Vector3(amount, 0, 0), duration);
    }
    
    public void RotateY(float amount)
    {
        transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, amount, 0), duration);
    }

    public void RotateZ(float amount)
    {
        transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, 0, amount), duration);
    }
}
