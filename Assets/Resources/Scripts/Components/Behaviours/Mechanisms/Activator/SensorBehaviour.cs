using UnityEngine;

public class SensorBehaviour : ActivatorBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        base.Activate(true);
    }
}
