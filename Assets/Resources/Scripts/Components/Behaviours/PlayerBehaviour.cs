using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Glossary;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Common references")]
    [SerializeField] Transform eyes;
    [SerializeField] public GameMode gameMode;
    [SerializeField] List<Ball> demoBalls = new();

    // Commons
    bool shot = false;
    List<Ball> balls = new();
    int selectedBall = 0;
    List<GameObject> auxiliarBalls = new();
    AudioSource audioSource;

    // VR commons
    GameObject projection;
    Transform wrist;
    Transform palm;
    bool aiming = false;

    // Desktop exclusive
    float speed = 2;
    float velForward;
    float velSide;

    [Header("Hands references")]
    [SerializeField] List<GameObject> handsExclusives = new();
    [SerializeField] GameObject H_projection;
    [SerializeField] Transform H_wrist;
    [SerializeField] Transform H_palm;
    [SerializeField] OVRHand H_hand;
    [SerializeField] AudioSource H_audioSource;

    [Header("Controllers references")]
    [SerializeField] List<GameObject> controllersExclusives = new();
    [SerializeField] GameObject C_projection;
    [SerializeField] Transform C_wrist;
    [SerializeField] Transform C_palm;
    [SerializeField] AudioSource C_audioSource;
    [SerializeField] GameObject C_grabInteractorL;
    [SerializeField] GameObject C_distanceGrabInteractorL;
    [SerializeField] GameObject C_grabInteractorR;
    [SerializeField] GameObject C_distanceGrabInteractorR;

    [Header("Desktop references")]
    [SerializeField] List<GameObject> desktopExclusives = new();
    [SerializeField] GameObject D_projection;
    [SerializeField] Transform D_wrist;
    [SerializeField] Transform D_palm;
    [SerializeField] AudioSource D_audioSource;
    [SerializeField] Rigidbody D_body;

    void Start()
    {
        SetGameMode(gameMode);

        // Add demo balls
        foreach (Ball ball in demoBalls) ObtainBall(ball);

        // Set default projection using first ball if it exists
        if (balls.Count < 0) projection.GetComponent<ProjectionBehaviour>().UpdateBall(balls[selectedBall]);
    }

    void SetGameMode(GameMode gm)
    {
        gameMode = gm;

        foreach (GameObject go in handsExclusives) go.SetActive(false);
        foreach (GameObject go in controllersExclusives) go.SetActive(false);
        foreach (GameObject go in desktopExclusives) go.SetActive(false);

        switch (gm)
        {
            case GameMode.Controllers:
                foreach (GameObject go in controllersExclusives) go.SetActive(true);
                projection = C_projection;
                wrist = C_wrist;
                palm = C_palm;
                audioSource = C_audioSource;
                break;

            case GameMode.Hands:
                foreach (GameObject go in handsExclusives) go.SetActive(true);
                projection = H_projection;
                wrist = H_wrist;
                palm = H_palm;
                audioSource = H_audioSource;
                break;

            case GameMode.Desktop:
                foreach (GameObject go in desktopExclusives) go.SetActive(true);
                projection = D_projection;
                wrist = D_wrist;
                palm = D_palm;
                audioSource = D_audioSource;
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

            if (InputManager.instance.Holding(InputManager.instance.shoot)) Shoot();

            if (InputManager.instance.Holding(InputManager.instance.aim)) palm.GetComponent<AimBeam>().Cast(balls[selectedBall]);
            else palm.GetComponent<AimBeam>().Clear();

            if (InputManager.instance.Pressed(InputManager.instance.swap)) SwitchBall(true);

            if (InputManager.instance.Pressed(InputManager.instance.clear)) ClearAuxiliars();
        }
        // Hands exclusive
        else if (gameMode == GameMode.Hands)
        {
            // While aiming...
            if (aiming)
            {
                // If no ball was recently shot...
                if (!shot)
                {
                    // Update the aim beam rendering
                    if (Vector3.Angle(eyes.forward, palm.forward) <= 90) palm.GetComponent<AimBeam>().Cast(balls[selectedBall]);
                    else palm.GetComponent<AimBeam>().Clear();

                    // Pinch index finger to shoot ball
                    if (H_hand.GetFingerIsPinching(OVRHand.HandFinger.Index)) Shoot();
                }
                else palm.GetComponent<AimBeam>().Clear();

                // Pinch middle finger to delete all auxiliar balls
                if (H_hand.GetFingerIsPinching(OVRHand.HandFinger.Middle)) ClearAuxiliars();
            }
            else palm.GetComponent<AimBeam>().Clear();
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
    }

    public void Shoot()
    {
        if (!shot) StartCoroutine(Shot());
    }

    public IEnumerator Shot()
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

    public void ClearAuxiliars()
    {
        foreach (GameObject ball in auxiliarBalls) Destroy(ball);
        auxiliarBalls.Clear();
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

    public void SelectBall(int idx)
    {
        selectedBall = idx;
        projection.GetComponent<ProjectionBehaviour>().UpdateBall(balls[selectedBall]);
    }

    #region Hands exclusive methods

    public void ShowProjection(bool on)
    {
        projection.SetActive(on);
    }

    public void OpenPalm(bool on)
    {
        aiming = on;

        if (on)
        {
            Preview(true, true);
        }
        else
        {
            SwitchBall(true);
            Preview(true, false);
        }
    }

    #endregion

    #region Desktop exclusive methods

    void FixedUpdate()
    {
        if (gameMode == GameMode.Desktop)
        {
            D_body.linearVelocity = new Vector3(speed * velSide, D_body.linearVelocity.y, speed * velForward);
        }
    }

    #endregion
}
