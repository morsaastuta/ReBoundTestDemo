using System.Collections.Generic;
using UnityEngine;

public class Suppressor : MonoBehaviour
{
    [SerializeField] List<Component> suppressedComponents = new();
    [SerializeField] List<GameObject> suppressedObjects = new();

    void LateUpdate()
    {
        foreach (Component c in suppressedComponents)
        {
            if (c.GetType().BaseType == typeof(Renderer)) ((Renderer)c).enabled = false;
            else if (c.GetType().BaseType.BaseType == typeof(Behaviour)) ((Behaviour)c).enabled = false;
        }
        foreach (GameObject go in suppressedObjects) go.SetActive(false);
    }
}
