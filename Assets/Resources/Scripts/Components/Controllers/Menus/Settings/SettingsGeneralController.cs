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
        subtitled.isOn = SubtitleManager.instance.subtitled;
        subtitleHeight.value = SubtitleManager.instance.subtitleHeight;
        subtitleDepth.value = SubtitleManager.instance.subtitleDepth;
    }

    public void SaveSettings()
    {
        SubtitleManager.instance.subtitled = subtitled.isOn;
        SubtitleManager.instance.subtitleDepth = subtitleDepth.value;
        SubtitleManager.instance.subtitleHeight = subtitleHeight.value;
    }
}
