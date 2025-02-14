using UnityEngine;
using static Glossary;

public class HittableUIBehaviour : MonoBehaviour
{
    [SerializeField] ButtonVRController button;

    void OnTriggerEnter(Collider collider)
    {
        if (CompareLayer(collider, Layer.Ball))
        {
            StartCoroutine(button.AutoButton(0.5f));
            Destroy(collider.gameObject);
        }
    }
}
