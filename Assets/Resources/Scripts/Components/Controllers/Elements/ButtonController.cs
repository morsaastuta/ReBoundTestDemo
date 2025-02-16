using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [Header("Customization")]
    [SerializeField] string title;

    [Header("References")]
    [SerializeField] TextMeshProUGUI text;

    private void Start()
    {
        text.SetText(title);
    }

    public void Press()
    {

    }
}
