using NaughtyAttributes;
using UnityEngine;
using static UnityEditor.Progress;

public class GloveBehaviour : MonoBehaviour
{
    [Header("Commons")]
    [SerializeField] public GameObject projection;
    [SerializeField] public Transform wrist;
    [SerializeField] public Transform palm;
    [SerializeField] public AudioSource audioSource;
    [SerializeField] Animator animator;

    [Header("Handtracking exclusives")]
    [SerializeField] public OVRHand hand;

    [Header("Desktop exclusives")]
    [ReadOnly] public Transform grabbedItem;

    public void Pose(int idx)
    {
        if (animator != null) animator.SetInteger("pose", idx);
    }

    public void GrabItem(Transform item)
    {
        DropItem();
        if (item != null && item.root == item)
        {
            grabbedItem = item;
            grabbedItem.GetComponent<Rigidbody>().isKinematic = true;
            grabbedItem.transform.position = palm.position;
            item.SetParent(transform);
        }
    }

    public void DropItem()
    {
        if (grabbedItem != null)
        {
            grabbedItem.SetParent(null);
            grabbedItem.GetComponent<Rigidbody>().isKinematic = false;
            grabbedItem = null;
        }
    }
}
