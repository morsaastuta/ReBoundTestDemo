using UnityEngine;

public class SettingsExitController : MonoBehaviour
{
    public void Exit()
    {
        GameManager.Instance.GoToScene(0);
    }
}
