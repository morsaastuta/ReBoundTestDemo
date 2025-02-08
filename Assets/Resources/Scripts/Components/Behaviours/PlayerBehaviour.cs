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
    [SerializeField] GameObject grabInteractorL;
    [SerializeField] GameObject distanceGrabInteractorL;
    [SerializeField] GameObject grabInteractorR;
    [SerializeField] GameObject distanceGrabInteractorR;
    [SerializeField] Transform eyes;
    [SerializeField] Transform palm;
    [SerializeField] AimBeam aimBeam;
    [SerializeField] Transform wrist;
    [SerializeField] GameObject projection;
    [SerializeField] AudioSource source;

    [Header("Desktop references")]
    [SerializeField] List<GameObject> desktopExclusive = new();
    [SerializeField] Rigidbody body;

    List<Ball> balls = new();
    int selectedBall = 0;
    List<GameObject> auxiliarBalls = new();
    bool shot = false;

    // Desktop exclusive
    float speed = 2;
    float velForward;
    float velSide;

    void Start()
    {
        // Add demo balls
        foreach (Ball ball in demoBalls) ObtainBall(ball);

        // Set default projection using first ball if it exists
        if (balls.Count < 0) projection.GetComponent<ProjectionBehaviour>().UpdateBall(balls[selectedBall]);
    }

    void SetGameMode(GameMode gm)
    {
        foreach (GameObject go in handsExclusive) go.SetActive(false);
        foreach (GameObject go in controllersExclusive) go.SetActive(false);

        switch (gm)
        {
            case GameMode.Hands:
                foreach (GameObject go in handsExclusive) go.SetActive(true);
                break;
            case GameMode.Controllers:
                foreach (GameObject go in controllersExclusive) go.SetActive(true);
                break;
            case GameMode.Desktop:
                foreach (GameObject go in desktopExclusive) go.SetActive(true);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Controllers exclusive
        if (gameMode == GameMode.Controllers)
        {
            if (InputManager.instance.Holding(InputManager.instance.distanceGrabL))
            {
                distanceGrabInteractorL.SetActive(true);
                grabInteractorL.SetActive(false);
            }
            else
            {
                distanceGrabInteractorL.SetActive(false);
                grabInteractorL.SetActive(true);
            }

            if (InputManager.instance.Holding(InputManager.instance.distanceGrabR))
            {
                distanceGrabInteractorR.SetActive(true);
                grabInteractorR.SetActive(false);
            }
            else
            {
                distanceGrabInteractorR.SetActive(false);
                grabInteractorR.SetActive(true);
            }
        }
        // Desktop exclusive
        else if (gameMode == GameMode.Desktop)
        {
            Vector2 vMove = InputManager.instance.Inclination(InputManager.instance.move);
            velSide = vMove.x;
            velForward = vMove.y;
            Debug.Log(vMove);

            Vector2 vLook = InputManager.instance.Inclination(InputManager.instance.look);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + vLook.x, transform.rotation.eulerAngles.z);
            eyes.rotation = Quaternion.Euler(eyes.rotation.eulerAngles.x + vLook.y, eyes.rotation.eulerAngles.y, eyes.rotation.eulerAngles.z);
            Debug.Log(vLook);
        }
        else return;


        // Common

        if (InputManager.instance.Holding(InputManager.instance.shoot)) Shoot();

        if (InputManager.instance.Holding(InputManager.instance.aim)) aimBeam.Cast(balls[selectedBall]);
        else aimBeam.Clear();

        if (InputManager.instance.Pressed(InputManager.instance.swap)) SwitchBall(true);

        if (InputManager.instance.Pressed(InputManager.instance.clear)) ClearAuxiliars();
    }

    public void SwitchBall(bool right)
    {
        //if (projection.activeInHierarchy && projection.transform.position == palm.position)
        //{
        if (balls.Count > 0)
        {
            if (right) selectedBall++;
            else selectedBall--;

            if (selectedBall < 0) selectedBall = balls.Count - 1;
            else if (selectedBall >= balls.Count) selectedBall = 0;

            projection.GetComponent<ProjectionBehaviour>().UpdateBall(balls[selectedBall]);
        }
        //}
    }

    void ClearAuxiliars()
    {
        foreach (GameObject ball in auxiliarBalls) Destroy(ball);
        auxiliarBalls.Clear();
    }

    void FixedUpdate()
    {
        if (gameMode == GameMode.Desktop)
        {
            body.linearVelocity = new Vector3(speed * velSide, body.linearVelocity.y, speed * velForward);
        }
    }

    void Shoot()
    {
        if (!shot) StartCoroutine(Shot());
    }

    IEnumerator Shot()
    {
        Preview(false, false);
        shot = true;

        GameObject ball = Instantiate(balls[selectedBall].prefab);

        // Set ball
        ball.GetComponent<BallBehaviour>().ball = Instantiate(balls[selectedBall]);

        // If ball is auxiliar, track it
        if (ball.GetComponent<BallBehaviour>().ball.auxiliar) auxiliarBalls.Add(ball);

        // Set position (Palm)
        ball.transform.position = palm.position;

        // Set rotation (Palm or Gyroscope)
        ball.transform.rotation = palm.rotation;

        yield return new WaitForSeconds(1f);

        shot = false;
        Preview(true, true);
    }

    public void ObtainBall(Ball ball)
    {
        balls.Add(ball);
        selectedBall = balls.Count - 1;
        projection.GetComponent<ProjectionBehaviour>().UpdateBall(balls[selectedBall]);
    }

    public void Preview(bool on, bool isPalm)
    {
        if (balls.Count > 0)
        {
            if (on)
            {
                if (isPalm)
                {
                    projection.transform.position = palm.position;
                    projection.transform.rotation = palm.rotation;
                    projection.transform.localScale = palm.localScale;
                }
                else
                {
                    projection.transform.position = wrist.position;
                    projection.transform.rotation = wrist.rotation;
                    projection.transform.localScale = wrist.localScale;
                }
                ShowProjection(true);
            }
            else ShowProjection(false);
        }
    }

    void ShowProjection(bool on)
    {
        projection.SetActive(on);
    }
}
