using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public OVROverlay overlay_Background;

    public static GameManager instance;

    [Header("Customization")]
    [SerializeField] public int sceneID = 0;

    [Header("References")]
    [SerializeField] Image loadScreen;

    public bool paused = false;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            instance.sceneID = sceneID;
            Destroy(gameObject);
            return;
        }
        else instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Pause(bool on)
    {
        if (on && !paused)
        {
            paused = true;
            //Time.timeScale = 0;
        }
        else if (!on && paused)
        {
            paused = false;
            //Time.timeScale = 1.0f;
        }
    }

    public void GoToScene(int id)
    {
        Pause(false);
        StartCoroutine(ShowOverlayAndLoad(id));
    }

    IEnumerator ShowOverlayAndLoad(int sceneIndex)
    {
        // activamos los componentes
        overlay_Background.enabled = true;
        GameObject centerEyeAnchor = GameObject.Find("CenterEyeAnchor");

        yield return new WaitForSeconds(4f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        overlay_Background.enabled = false;
        yield return null;
    }

    public void QuitGame()
    {
        Pause(false);
        StartCoroutine(QuitTransition());
    }

    IEnumerator QuitTransition()
    {
        loadScreen.DOColor(new Color(0, 0, 0, 1), 2f);
        yield return new WaitForSeconds(2.5f);
        Application.Quit();
    }
}