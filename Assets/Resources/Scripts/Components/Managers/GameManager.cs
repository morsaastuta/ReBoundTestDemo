using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
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
        StartCoroutine(SceneTransition(id));
    }

    IEnumerator SceneTransition(int id)
    {
        loadScreen.DOColor(new Color(0,0,0,1), 0.5f);
        yield return new WaitForSeconds(0.6f);
        SceneManager.LoadSceneAsync("Scene" + id.ToString("00"));
        yield return new WaitForSeconds(1.4f);
        loadScreen.DOColor(new Color(0, 0, 0, 0), 0.5f);
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