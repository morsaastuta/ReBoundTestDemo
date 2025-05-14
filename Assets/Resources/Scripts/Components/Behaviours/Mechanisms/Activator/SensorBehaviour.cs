using UnityEngine;

public class SensorBehaviour : ActivatorBehaviour
{
    [TagSelector][SerializeField] string tagName;

    bool once = false;

    public override void Activate(bool on)
    {
        if (!once)
        {
            if (on) once = true;
            base.Activate(on);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(tagName))
        {
            Activate(true);
        }
    }
}
