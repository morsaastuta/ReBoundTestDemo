using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool paused = false;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

    public void Pause(bool on)
    {
        if (on && !paused)
        {
            paused = true;
            Time.timeScale = 0;
        }
        else if (!on && paused)
        {
            paused = false;
            Time.timeScale = 1.0f;
        }
    }
}