using UnityEngine;

public class SensorBehaviour : ActivatorBehaviour
{
    [TagSelector][SerializeField] string tagName;
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(tagName))
            base.Activate(true);
    }
}
