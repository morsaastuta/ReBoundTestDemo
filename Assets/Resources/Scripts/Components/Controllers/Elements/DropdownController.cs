using TMPro;
using UnityEngine;

public class DropdownController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TMP_Dropdown dropdown;

    void FixedUpdate()
    {
        if (GameObject.Find("Blocker") != null) Destroy(GameObject.Find("Blocker"));
    }

    public int GetIndex()
    {
        return dropdown.value;
    }

    public void SetIndex(int idx)
    {
        dropdown.value = idx;
    }
}
