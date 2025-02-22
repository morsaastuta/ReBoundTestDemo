using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    [Header("References")]
    [SerializeField] Canvas canvas;
    [SerializeField] TextMeshProUGUI subtitleMesh;

    // Settings
    public bool subtitled = true;
    public float subtitleDistance = 0.9f;
    public float subtitleHeight = 64f;

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

    void Update()
    {
        SetSubtitles("aaaaaaaaaaaaa");
    }

    public void SetSubtitles(string content)
    {
        canvas.worldCamera = Camera.main;
        canvas.planeDistance = subtitleDistance;
        subtitleMesh.rectTransform.anchoredPosition = new Vector2(0, subtitleHeight);

        subtitleMesh.text = content;
    }

    public void ClearSubtitles()
    {
        subtitleMesh.text = "";
    }
}