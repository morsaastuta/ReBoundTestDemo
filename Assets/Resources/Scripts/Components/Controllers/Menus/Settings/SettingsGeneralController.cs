using UnityEngine;
using UnityEngine.UI;

public class SettingsGeneralController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Toggle subtitled;
    [SerializeField] Slider subtitleHeight;
    [SerializeField] Slider subtitleDepth;

    void OnEnable()
    {
        subtitled.isOn = EventManager.instance.subtitled;
        subtitleHeight.value = EventManager.instance.subtitleHeight;
        subtitleDepth.value = EventManager.instance.subtitleDepth;
    }

    public void SaveSettings()
    {
        EventManager.instance.subtitled = subtitled.isOn;
        EventManager.instance.subtitleDepth = subtitleDepth.value;
        EventManager.instance.subtitleHeight = subtitleHeight.value;
    }
}
