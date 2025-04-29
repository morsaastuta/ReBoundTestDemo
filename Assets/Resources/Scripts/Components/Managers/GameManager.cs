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
    [SerializeField] GameObject loadCanvas;

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

    void Update()
    {
        if (GameObject.Find("CenterEyeAnchor") != null)
        {
            Transform eyes = GameObject.Find("CenterEyeAnchor").transform;
            loadCanvas.transform.position = new(eyes.transform.position.x, eyes.transform.position.y, eyes.transform.position.z + 1);
        }
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
        Image tempImg = loadCanvas.GetComponent<Image>();

        tempImg.DOColor(new Color(0, 0, 0, 1), 2f);
        yield return new WaitForSeconds(2f);

        yield return SceneManager.LoadSceneAsync(sceneIndex);

        tempImg.DOColor(new Color(0, 0, 0, 0), 2f);
        yield return new WaitForSeconds(2f);
    }

    public void QuitGame()
    {
        Pause(false);
        StartCoroutine(QuitTransition());
    }

    IEnumerator QuitTransition()
    {
        yield return new WaitForSeconds(2f);
        Application.Quit();
    }
}