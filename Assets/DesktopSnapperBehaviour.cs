using UnityEngine;
using static Glossary;

public class DesktopSnapperBehaviour : MonoBehaviour
{
    [SerializeField] Transform snappos;
    Transform snappedItem;

    bool DesktopMode
    {
        get
        {
            return GameObject.Find("Player").GetComponent<PlayerBehaviour>().gameMode == GameMode.Desktop;
        }
    }

    void OnTriggerStay(Collider item)
    {
        if (DesktopMode)
        {
            if (item.transform.root.gameObject.layer == LayerMask.NameToLayer("Item"))
            {
                if (!item.attachedRigidbody.isKinematic)
                {
                    SnapItem(item.transform.root);
                }
            }
        }
    }

    void OnTriggerExit(Collider item)
    {
        if (DesktopMode)
        {
            snappedItem.SetParent(null);
            snappedItem = null;
        }
    }

    public void SnapItem(Transform item)
    {
        DropItem();
        if (item != null && item.root == item)
        {
            snappedItem = item;
            snappedItem.GetComponent<Rigidbody>().isKinematic = true;
            snappedItem.transform.position = snappos.position;
            snappedItem.transform.rotation = snappos.rotation;
            item.SetParent(transform);
        }
    }

    public void DropItem()
    {
        if (snappedItem != null)
        {
            snappedItem.SetParent(null);
            snappedItem.GetComponent<Rigidbody>().isKinematic = false;
            snappedItem = null;
        }
    }
}
