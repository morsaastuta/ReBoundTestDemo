using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuCreditsBehaviour : MonoBehaviour
{
    [Header("Customization")]
    [SerializeField] string member;
    [SerializeField] List<string> work;

    [Header("References")]
    [SerializeField] TextMeshPro memberText;
    [SerializeField] TextMeshPro workText;

    void Start()
    {
        memberText.SetText(member);
        foreach (string job in work) workText.SetText(workText.text + job + "\n");
    }
}
