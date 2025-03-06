using Unity.VisualScripting;
using UnityEngine;


public class DestroyerBehaviour : CheckerBehaviour
{
    [TagSelector] [SerializeField] string tagName;
    [SerializeField] float timeToDestroy = 0.0f;

    [DoNotSerialize] public int quantityDestroyed;

    void OnTriggerEnter(Collider other)
    {
        quantityDestroyed++;
        if (other.CompareTag(tagName)) 
            Destroy(other.gameObject, timeToDestroy);
    }

    public override bool Check(int rule)
    {
        return quantityDestroyed >= rule;
    }
}
