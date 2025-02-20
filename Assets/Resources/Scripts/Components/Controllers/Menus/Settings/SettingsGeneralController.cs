using UnityEngine;
using UnityEngine.UI;

public class SettingsGeneralController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Toggle subtitlesOn;
    [SerializeField] Slider subtitleDistance;
    [SerializeField] Slider subtitleHeight;

    public void SaveSettings()
    {
        EventManager.instance.subtitled = false;
        EventManager.instance.subtitleDistance = subtitleDistance.value;
        EventManager.instance.subtitleHeight = subtitleHeight.value;
    }
}
