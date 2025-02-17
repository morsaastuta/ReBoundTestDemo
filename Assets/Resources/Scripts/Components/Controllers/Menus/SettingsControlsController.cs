using Oculus.Interaction.Samples;
using UnityEngine;
using UnityEngine.UI;
using static Glossary;

public class SettingsControlsController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Toggle leftToggle;
    [SerializeField] DropDownGroup controlScheme;

    private void OnEnable()
    {
        PlayerBehaviour player = GameObject.Find("Player").GetComponent<PlayerBehaviour>();

        leftToggle.enabled = player.leftMode;
    }

    public void SaveSettings()
    {
        PlayerBehaviour player = GameObject.Find("Player").GetComponent<PlayerBehaviour>();

        if (!leftToggle.isOn) player.SetHandedness(false);
        else player.SetHandedness(true);

        switch (controlScheme.SelectedIndex)
        {
            case 0: player.SetGameMode(GameMode.Hands); break;
            case 1: player.SetGameMode(GameMode.Controllers); break;
            case 2: player.SetGameMode(GameMode.Desktop); break;
        }
    }
}
