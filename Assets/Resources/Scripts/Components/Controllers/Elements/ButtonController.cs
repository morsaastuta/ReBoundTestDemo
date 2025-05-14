using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour, IPointerEnterHandler
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
        if (icon != null && image != null) image.sprite = icon;
    }

    public void Press()
    {
        Debug.Log("pressed " + title);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("im here");
    }
}
