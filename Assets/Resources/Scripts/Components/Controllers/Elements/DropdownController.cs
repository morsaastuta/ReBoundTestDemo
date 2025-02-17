using TMPro;
using UnityEngine;

public class DropdownController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;

    public void SetTitle(string t)
    {
        title.text = t;
    }
}
