using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Glossary;

public class SettingsControlsController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Toggle leftToggle;
    [SerializeField] TMP_Dropdown controlScheme;

    private void OnEnable()
    {
        PlayerBehaviour player = GameObject.Find("Player").GetComponent<PlayerBehaviour>();

        leftToggle.enabled = player.leftMode;

        switch (player.gameMode)
        {
            case GameMode.Hands: controlScheme.value = 0; break;
            case GameMode.Controllers: controlScheme.value = 1; break;
            case GameMode.Desktop: controlScheme.value = 2; break;
        }
    }

    public void SaveSettings()
    {
        PlayerBehaviour player = GameObject.Find("Player").GetComponent<PlayerBehaviour>();

        if (!leftToggle.isOn) player.SetHandedness(false);
        else player.SetHandedness(true);

        switch (controlScheme.value)
        {
            case 0: player.SetGameMode(GameMode.Hands); break;
            case 1: player.SetGameMode(GameMode.Controllers); break;
            case 2: player.SetGameMode(GameMode.Desktop); break;
        }
    }
}
