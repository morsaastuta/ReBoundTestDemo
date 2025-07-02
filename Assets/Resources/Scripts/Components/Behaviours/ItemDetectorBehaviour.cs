using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class ItemDetectorBehaviour : MonoBehaviour
{
    [SerializeField][ReadOnly] public List<Transform> detectedItems = new();

    void Update()
    {
        List<int> itemsToRemove = new();

        foreach (Transform item in detectedItems) if (item == null) itemsToRemove.Add(detectedItems.IndexOf(item));

        foreach (int idx in itemsToRemove) detectedItems.RemoveAt(idx);
    }

    void OnTriggerEnter(Collider item)
    {
        if (item.transform.root.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            detectedItems.Add(item.transform.root);
        }
    }

    void OnTriggerExit(Collider item)
    {
        if (item.transform.root.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            detectedItems.Remove(item.transform.root);
        }
    }

    public void GetItem(GloveBehaviour hand)
    {
        if (hand.grabbedItem == null && detectedItems.Count > 0)
        {
            Vector3 pos = hand.transform.position;
            Vector3 shortestDis = new(999, 999, 999);
            int shortestIdx = 0;

            foreach (Transform item in detectedItems)
            {
                Vector3 itemDis = item.transform.position - pos;
                if (itemDis.magnitude < shortestDis.magnitude)
                {
                    shortestDis = itemDis;
                    shortestIdx = detectedItems.IndexOf(item);
                }
            }

            Transform grabbedItem = detectedItems[shortestIdx];
            detectedItems.RemoveAt(shortestIdx);
            hand.GrabItem(grabbedItem);
        }
    }

}
