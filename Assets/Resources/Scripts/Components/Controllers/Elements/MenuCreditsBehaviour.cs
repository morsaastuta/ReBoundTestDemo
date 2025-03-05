using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuCreditsBehaviour : MonoBehaviour
{
    [Header("Customization")]
    [SerializeField] string user;
    [SerializeField] List<string> work;
    [SerializeField] string url;

    [Header("References")]
    [SerializeField] TextMeshPro userText;
    [SerializeField] TextMeshPro workText;

    void Start()
    {
        userText.SetText(user);
        foreach (string job in work) workText.SetText(workText.text + job + "\n");
    }

    public void Open()
    {
        if (url != null && url.Length > 0) Application.OpenURL("http://" + url);
    }
}
