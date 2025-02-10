using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Glossary;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] public GameMode gameMode;
    [SerializeField] List<Ball> demoBalls = new();

    [Header("Hands references")]
    [SerializeField] List<GameObject> handsExclusive = new();

    [Header("Controllers references")]
    [SerializeField] List<GameObject> controllersExclusive = new();
    [SerializeField] GloveBehaviour C_glove;
    [SerializeField] GameObject C_grabInteractorL;
    [SerializeField] GameObject C_distanceGrabInteractorL;
    [SerializeField] GameObject C_grabInteractorR;
    [SerializeField] GameObject C_distanceGrabInteractorR;
    [SerializeField] AudioSource C_audioSource;

    [Header("Desktop references")]
    [SerializeField] List<GameObject> desktopExclusive = new();
    [SerializeField] Rigidbody D_body;
    [SerializeField] AudioSource D_audioSource;

    List<Ball> balls = new();
    int selectedBall = 0;
    List<GameObject> auxiliarBalls = new();
    bool shot = false;

    AudioSource currentSource;

    // Desktop exclusive
    float speed = 2;
    float velForward;
    float velSide;

    void Start()
    {
        SetGameMode(gameMode);
    }

    void SetGameMode(GameMode gm)
    {
        gameMode = gm;

        foreach (GameObject go in handsExclusive) go.SetActive(false);
        foreach (GameObject go in controllersExclusive) go.SetActive(false);

        switch (gm)
        {
            case GameMode.Hands:
                foreach (GameObject go in handsExclusive) go.SetActive(true);
                break;
            case GameMode.Controllers:
                foreach (GameObject go in controllersExclusive) go.SetActive(true);
                currentSource = C_audioSource;
                break;
            case GameMode.Desktop:
                foreach (GameObject go in desktopExclusive) go.SetActive(true);
                currentSource = D_audioSource;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameMode != GameMode.Hands)
        {
            // Controllers exclusive
            if (gameMode == GameMode.Controllers)
            {
                if (InputManager.instance.Holding(InputManager.instance.distanceGrabL))
                {
                    C_distanceGrabInteractorL.SetActive(true);
                    C_grabInteractorL.SetActive(false);
                }
                else
                {
                    C_distanceGrabInteractorL.SetActive(false);
                    C_grabInteractorL.SetActive(true);
                }

                if (InputManager.instance.Holding(InputManager.instance.distanceGrabR))
                {
                    C_distanceGrabInteractorR.SetActive(true);
                    C_grabInteractorR.SetActive(false);
                }
                else
                {
                    C_distanceGrabInteractorR.SetActive(false);
                    C_grabInteractorR.SetActive(true);
                }

                if (InputManager.instance.Holding(InputManager.instance.shoot)) C_glove.Shoot();

                if (InputManager.instance.Holding(InputManager.instance.aim)) C_glove.aimBeam.Cast(balls[selectedBall]);
                else C_glove.aimBeam.Clear();

                if (InputManager.instance.Pressed(InputManager.instance.swap)) C_glove.SwitchBall(true);

                if (InputManager.instance.Pressed(InputManager.instance.clear)) C_glove.ClearAuxiliars();
            }
            // Desktop exclusive
            else if (gameMode == GameMode.Desktop)
            {
                Vector2 vMove = InputManager.instance.Inclination(InputManager.instance.move);
                velSide = vMove.x;
                velForward = vMove.y;

                Vector2 vLook = InputManager.instance.Inclination(InputManager.instance.look);
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + vLook.x, transform.rotation.eulerAngles.z);
            }
            else return;
        }
    }

    void FixedUpdate()
    {
        if (gameMode == GameMode.Desktop)
        {
            D_body.linearVelocity = new Vector3(speed * velSide, D_body.linearVelocity.y, speed * velForward);
        }
    }
}
