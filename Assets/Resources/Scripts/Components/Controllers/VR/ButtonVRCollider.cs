using UnityEngine;
using static Glossary;

public class ButtonVRCollider : MonoBehaviour
{
    [SerializeField] ButtonVRController root;

    void OnTriggerEnter(Collider collider)
    {
        if (CompareLayer(collider, Layer.Ball))
        {
            StartCoroutine(root.AutoButton());
            Destroy(collider.gameObject);
        }
    }
}
