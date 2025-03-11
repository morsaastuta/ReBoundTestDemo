using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MenuVRController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] List<ButtonVRController> buttons;

    [Header("Settings")]
    [SerializeField] List<GameObject> settingsExclusive;

    [Header("Credits")]
    [SerializeField] List<GameObject> creditsExclusive;
    [SerializeField] List<MeshRenderer> walls = new();
    [SerializeField] Material white;
    [SerializeField] Material transparent;

    void Start()
    {
        ResetAll();
    }

    public void ResetAll()
    {
        GameManager.instance.paused = false;
        foreach (GameObject go in settingsExclusive) go.SetActive(false);
        foreach (GameObject go in creditsExclusive) go.SetActive(false);

        foreach (MeshRenderer wall in walls) wall.material = transparent;
        foreach (ButtonVRController button in buttons)
        {
            button.gameObject.SetActive(true);
            button.ButtonUnpress();
        }
    }

    public void Play()
    {
        ResetAll();
        GameManager.instance.GoToScene(1);
    }

    public void Settings()
    {
        ResetAll();
        GameManager.instance.paused = true;
        foreach (GameObject go in settingsExclusive) go.SetActive(true);
    }

    public void Credits()
    {
        ResetAll();
        foreach (ButtonVRController button in buttons) button.gameObject.SetActive(false);
        foreach (GameObject go in creditsExclusive) go.SetActive(true);
        foreach (MeshRenderer wall in walls) wall.material = white;
    }

    public void Exit()
    {
        ResetAll();
        GameManager.instance.QuitGame();
    }
}
