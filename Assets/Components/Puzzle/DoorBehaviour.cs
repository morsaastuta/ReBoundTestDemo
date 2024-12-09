using DG.Tweening;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    [Header("Customization")]
    [SerializeField] ButtonBehaviour assignedButton;
    [SerializeField] float rotationAngles = 90;
    [SerializeField] bool clockwise = true;
    [SerializeField] float duration = 1;

    bool active = false;

    void FixedUpdate()
    {
        if (assignedButton != null)
        {
            if (assignedButton.pressed && !active)
            {
                active = true;

                if (!clockwise) transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, -rotationAngles, 0), duration);
                else transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, rotationAngles, 0), duration);
            }
            else if (!assignedButton.pressed && active)
            {
                active = false;

                if (!clockwise) transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, rotationAngles, 0), duration);
                else transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, -rotationAngles, 0), duration);
            }
        }
    }
}
