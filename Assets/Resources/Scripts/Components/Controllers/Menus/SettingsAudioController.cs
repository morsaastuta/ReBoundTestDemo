using UnityEngine;
using UnityEngine.UI;

public class SettingsAudioController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Slider globalSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundSlider;
    [SerializeField] Slider voiceSlider;

    public void SaveSettings()
    {
        AudioManager.instance.SetVolume(globalSlider.value, musicSlider.value, soundSlider.value, voiceSlider.value);
    }
}
