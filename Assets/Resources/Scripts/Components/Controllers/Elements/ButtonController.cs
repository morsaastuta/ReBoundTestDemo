using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [Header("Customization")]
    [SerializeField] string title;
    [SerializeField] Sprite icon;

    [Header("References")]
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Image image;

    void Start()
    {
        text.SetText(title);
        image.sprite = icon;
    }

    public void Press()
    {

    }
}
