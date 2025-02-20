using Oculus.Interaction.Samples;
using UnityEngine;
using UnityEngine.UI;
using static Glossary;

public class SettingsControlsController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Toggle leftToggle;
    [SerializeField] DropdownController controlScheme;

    private void OnEnable()
    {
        PlayerBehaviour player = GameObject.Find("Player").GetComponent<PlayerBehaviour>();

        leftToggle.isOn = player.leftMode;

        switch (player.gameMode)
        {
            case GameMode.Hands: controlScheme.SetIndex(0); break;
            case GameMode.Controllers: controlScheme.SetIndex(1); break;
            case GameMode.Desktop: controlScheme.SetIndex(2); break;
        }
    }

    public void SaveSettings()
    {
        PlayerBehaviour player = GameObject.Find("Player").GetComponent<PlayerBehaviour>();

        player.SetHandedness(leftToggle.isOn);

        switch (controlScheme.GetIndex())
        {
            case 0: player.SetGameMode(GameMode.Hands); break;
            case 1: player.SetGameMode(GameMode.Controllers); break;
            case 2: player.SetGameMode(GameMode.Desktop); break;
        }
    }
}
