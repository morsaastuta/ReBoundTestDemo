using UnityEngine;

public class SettingsController : MonoBehaviour
{
    [Header("Panes")]
    [SerializeField] GameObject paneGeneral;
    [SerializeField] GameObject paneControls;
    [SerializeField] GameObject paneAudio;
    [SerializeField] GameObject paneExit;

    public void ResetAll()
    {
        paneGeneral.SetActive(false);
        paneControls.SetActive(false);
        paneAudio.SetActive(false);
        paneExit.SetActive(false);
    }

    public void PaneGeneral()
    {
        ResetAll();
        paneGeneral.SetActive(true);
    }

    public void PaneControls()
    {
        ResetAll();
        paneControls.SetActive(true);
    }

    public void PaneAudio()
    {
        ResetAll();
        paneAudio.SetActive(true);
    }

    public void PaneExit()
    {
        ResetAll();
        paneExit.SetActive(true);
    }
}
