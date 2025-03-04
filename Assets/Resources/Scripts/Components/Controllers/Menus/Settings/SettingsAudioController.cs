using UnityEngine;
using UnityEngine.UI;

public class SettingsAudioController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Slider globalSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundSlider;
    [SerializeField] Slider voiceSlider;

    void OnEnable()
    {
        globalSlider.value = AudioManager.instance.globalVolume;
        musicSlider.value = AudioManager.instance.musicVolume;
        soundSlider.value = AudioManager.instance.soundVolume;
        voiceSlider.value = AudioManager.instance.voiceVolume;
    }

    public void SaveSettings()
    {
        AudioManager.instance.SetVolume(globalSlider.value, musicSlider.value, soundSlider.value, voiceSlider.value);
    }
}
