using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class MovableObjectBehaviour : MonoBehaviour
{
    [Header("Customization")]
    [SerializeField] ButtonBehaviour assignedButton;
    
    [Header("Position")]
    [SerializeField] Transform newTransform;
    [SerializeField] Vector3 newPosition = Vector3.zero;

    Transform firstTransform;


    [SerializeField] float duration = 1;

    bool active = false;

    private void Awake()
    {
        firstTransform = this.transform;
    }

    void FixedUpdate()
    {
        if (assignedButton != null)
        {
            if (assignedButton.pressed && !active)
            {
                active = true;

                if (newTransform != null)
                    transform.DOMove(newTransform.position, duration);
                else
                    transform.DOMove(newPosition, duration);
                
            }
            else if (!assignedButton.pressed && active)
            {
                active = false;
                transform.DOMove(firstTransform.position, duration);
            }
        }
    }
}
