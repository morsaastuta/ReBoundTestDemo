using UnityEngine;

public class MenuVRController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ButtonVRController buttonSettings;
    [SerializeField] GameObject canvasSettings;

    public void ResetAll()
    {
        buttonSettings.ButtonUnpress();
        canvasSettings.SetActive(false);
    }

    public void CanvasSettings()
    {
        ResetAll();
        canvasSettings.SetActive(true);
    }

    public void Play()
    {
        ResetAll();
        GameManager.instance.GoToScene(1);
    }

    public void Exit()
    {
        ResetAll();
        GameManager.instance.QuitGame();
    }
}
