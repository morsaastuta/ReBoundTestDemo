using UnityEngine;

public class SettingsController : MonoBehaviour
{
    [Header("Panes")]
    [SerializeField] GameObject paneControls;
    [SerializeField] GameObject paneAudio;
    [SerializeField] GameObject paneExit;

    public void ResetAll()
    {
        paneControls.SetActive(false);
        paneAudio.SetActive(false);
        paneExit.SetActive(false);
    }

    public void PaneControls()
    {
        ResetAll();
        paneControls.SetActive(true);
        Debug.Log("happening");
    }

    public void PaneAudio()
    {
        ResetAll();
        paneAudio.SetActive(true);
        Debug.Log("happening");
    }

    public void PaneExit()
    {
        ResetAll();
        paneExit.SetActive(true);
        Debug.Log("happening");
    }
}
