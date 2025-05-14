using Oculus.Haptics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Glossary;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Common references")]
    [SerializeField] Transform eyes;
    [SerializeField] List<Ball> demoBalls = new();

    // Commons
    public GameMode gameMode;
    public bool leftMode;
    bool shot = false;
    List<Ball> balls = new();
    int selectedBall = 0;
    List<GameObject> auxiliarBalls = new();
    GloveBehaviour glove;
    GloveBehaviour hand;

    [Header("VR references")]
    [SerializeField] GameObject canvasVR;
    [SerializeField] Transform canvasVRPos;

    [Header("Constrictions")]
    bool gloveOn = true;
    [SerializeField] List<GameObject> teleportInteractors = new();

    // VR commons
    Controller gloveHand;
    bool aiming = false;
    GameObject canvasVRInstance;

    // Desktop exclusives
    float speed = 2;
    float velForward;
    float velSide;

    [Header("SFX")]
    [SerializeField] AudioClip shootClip;
    [SerializeField] AudioClip swapClip;
    [SerializeField] AudioClip clearClip;

    [Header("Handedness references")]
    [SerializeField] List<GameObject> rightExclusive = new();
    [SerializeField] List<GameObject> leftExclusive = new();

    [Header("Hands references")]
    [SerializeField] List<GameObject> handsExclusives = new();
    [SerializeField] GloveBehaviour HR_glove;
    [SerializeField] GloveBehaviour HL_glove;

    [Header("Controllers references")]
    [SerializeField] List<GameObject> controllersExclusives = new();
    [SerializeField] GloveBehaviour CR_glove;
    [SerializeField] GloveBehaviour CL_glove;
    [SerializeField] GloveBehaviour CR_hand;
    [SerializeField] GloveBehaviour CL_hand;
    [SerializeField] GameObject C_grabInteractorL;
    [SerializeField] GameObject C_distanceGrabInteractorL;
    [SerializeField] GameObject C_grabInteractorR;
    [SerializeField] GameObject C_distanceGrabInteractorR;

    [Header("Desktop references")]
    [SerializeField] List<GameObject> desktopExclusives = new();
    [SerializeField] GloveBehaviour DR_glove;
    [SerializeField] GloveBehaviour DL_glove;
    [SerializeField] GloveBehaviour DR_hand;
    [SerializeField] GloveBehaviour DL_hand;
    [SerializeField] Rigidbody D_body;
    [SerializeField] GameObject D_pauseMenu;
    [SerializeField] Transform D_eyes;
    [SerializeField] ItemDetectorBehaviour D_itemDetector;

    void Start()
    {
        SetGameMode(gameMode);
        SetHandedness(leftMode);
        UpdateComponents();

        // Add demo balls
        foreach (Ball ball in demoBalls) ObtainBall(ball);

        // Set default projection using first ball if it exists
        if (balls.Count > 0) glove.projection.GetComponent<ProjectionBehaviour>().UpdateBall(balls[selectedBall], gloveOn);
    }

    public void AmbientFeedback(HapticClip feedback)
    {
        HapticsManager.instance.Play(feedback, Controller.Both);
    }

    public void SetGameMode(GameMode gm)
    {
        gameMode = gm;
        UpdateComponents();
    }

    public void SetHandedness(bool left)
    {
        leftMode = left;

        if (!leftMode) gloveHand = Controller.Right;
        else gloveHand = Controller.Left;

        InputManager.Instance.SetHandedness(leftMode);

        UpdateComponents();
    }

    public void GloveOn(bool on)
    {
        gloveOn = on;
        if (balls.Count > 0) glove.projection.GetComponent<ProjectionBehaviour>().UpdateBall(balls[selectedBall], gloveOn);
    }

    public void TeleportOn(bool on)
    {
        foreach (GameObject teleportInteractor in teleportInteractors)
        {
            teleportInteractor.SetActive(on);
        }
    }

    void UpdateComponents()
    {
        // GAME MODE
        foreach (GameObject go in handsExclusives) go.SetActive(false);
        foreach (GameObject go in controllersExclusives) go.SetActive(false);
        foreach (GameObject go in desktopExclusives) go.SetActive(false);

        switch (gameMode)
        {
            case GameMode.Hands:
                foreach (GameObject go in handsExclusives) go.SetActive(true);
                if (!leftMode) glove = HR_glove;
                else glove = HL_glove;
                break;

            case GameMode.Controllers:
                foreach (GameObject go in controllersExclusives) go.SetActive(true);
                if (!leftMode)
                {
                    glove = CR_glove;
                    hand = CL_hand;
                }
                else
                {
                    glove = CL_glove;
                    hand = CR_hand;
                }
                break;

            case GameMode.Desktop:
                foreach (GameObject go in desktopExclusives) go.SetActive(true);

                if (!leftMode)
                {
                    hand = DL_hand;
                    glove = DR_glove;
                }
                else
                {
                    glove = DL_glove;
                    hand = DR_hand;
                }
                break;
        }

        // HANDEDNESS
        foreach (GameObject go in leftExclusive) go.SetActive(leftMode);
        foreach (GameObject go in rightExclusive) go.SetActive(!leftMode);
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameMode)
        {
            // Controllers exclusive
            case GameMode.Controllers:

                if (InputManager.Instance.Holding(InputManager.Instance.distanceGrabL))
                {
                    C_distanceGrabInteractorL.SetActive(true);
                    C_grabInteractorL.SetActive(false);
                }
                else
                {
                    C_distanceGrabInteractorL.SetActive(false);
                    C_grabInteractorL.SetActive(true);
                }

                if (InputManager.Instance.Holding(InputManager.Instance.distanceGrabR))
                {
                    C_distanceGrabInteractorR.SetActive(true);
                    C_grabInteractorR.SetActive(false);
                }
                else
                {
                    C_distanceGrabInteractorR.SetActive(false);
                    C_grabInteractorR.SetActive(true);
                }

                if (InputManager.Instance.Holding(InputManager.Instance.aim)) ProjectAimBeam(gloveOn);
                else ProjectAimBeam(false);

                break;

            // Hands exclusive
            case GameMode.Hands:

                // While aiming...
                if (aiming)
                {
                    // If no ball was recently shot...
                    if (!shot)
                    {
                        // Update the aim beam rendering
                        if (Vector3.Angle(eyes.forward, glove.palm.forward) <= 90) ProjectAimBeam(gloveOn);
                        else ProjectAimBeam(false);

                        // Pinch index finger to shoot ball
                        if (glove.hand.GetFingerIsPinching(OVRHand.HandFinger.Index)) Shoot();
                    }
                    else ProjectAimBeam(false);

                    // Pinch middle finger to delete all auxiliar balls
                    if (glove.hand.GetFingerIsPinching(OVRHand.HandFinger.Middle)) ClearAuxiliars();
                }
                else ProjectAimBeam(false);

                break;

            // Desktop exclusive
            case GameMode.Desktop:

                Vector2 vMove = Vector2.zero;
                Vector2 vLook = Vector2.zero;

                if (!GameManager.Instance.paused)
                {
                    vMove = InputManager.Instance.Inclination(InputManager.Instance.move);

                    vLook = InputManager.Instance.Inclination(InputManager.Instance.look) / 3.5f;
                    D_body.rotation = Quaternion.Euler(D_body.rotation.eulerAngles.x, D_body.rotation.eulerAngles.y + vLook.x, D_body.rotation.eulerAngles.z);
                    float eyeRot = D_eyes.rotation.eulerAngles.x - vLook.y;
                    if (eyeRot > 60 && eyeRot < 300)
                    {
                        if (vLook.y < 0) eyeRot = 59;
                        else eyeRot = 300;
                    }
                    D_eyes.rotation = Quaternion.Euler(eyeRot, D_eyes.rotation.eulerAngles.y, D_eyes.rotation.eulerAngles.z);

                    if (InputManager.Instance.Holding(InputManager.Instance.aim)) ProjectAimBeam(gloveOn);
                    else ProjectAimBeam(false);

                    if (InputManager.Instance.Pressed(InputManager.Instance.grabL)) D_itemDetector.GetItem(hand);
                    else if (InputManager.Instance.Released(InputManager.Instance.grabL)) hand.DropItem();

                    if (InputManager.Instance.Pressed(InputManager.Instance.grabR)) D_itemDetector.GetItem(glove);
                    else if (InputManager.Instance.Released(InputManager.Instance.grabR)) glove.DropItem();
                }

                velSide = vMove.x;
                velForward = vMove.y;

                break;
        }

        // Normal cycle if the game is not paused. If it is, keep cycle only while VR
        if (!GameManager.Instance.paused || gameMode != GameMode.Desktop)
        {
            if (InputManager.Instance.Holding(InputManager.Instance.shoot)) Shoot();

            if (InputManager.Instance.Holding(InputManager.Instance.grabL) || InputManager.Instance.Holding(InputManager.Instance.distanceGrabL))
            {
                if (leftMode) glove.Pose(1);
                else hand.Pose(1);
            }
            else if (!shot && !aiming)
            {
                if (leftMode) glove.Pose(0);
                else hand.Pose(0);
            }

            if (InputManager.Instance.Holding(InputManager.Instance.grabR) || InputManager.Instance.Holding(InputManager.Instance.distanceGrabR))
            {
                if (!leftMode) glove.Pose(1);
                else hand.Pose(1);
            }
            else if (!shot && !aiming)
            {
                if (!leftMode) glove.Pose(0);
                else hand.Pose(0);
            }

            if (InputManager.Instance.Pressed(InputManager.Instance.swap)) SwapBall(true);

            if (InputManager.Instance.Pressed(InputManager.Instance.clear)) ClearAuxiliars();
        }
        else
        {

        }

        // Can't pause on MainMenu (ID 0)
        if (GameManager.Instance.sceneID != 0 && InputManager.Instance.Pressed(InputManager.Instance.pause)) StartCoroutine(CallPause());
    }

    public void Shoot()
    {
        if (!GameManager.Instance.paused && gloveOn && !shot && balls.Count > 0)
        {
            HapticsManager.instance.Play(HapticsManager.instance.gloveFeedback, gloveHand);
            AudioManager.instance.PlaySound(shootClip, glove.audioSource);
            StartCoroutine(Shot());
        }
    }

    void ProjectAimBeam(bool on)
    {
        aiming = on;

        if (gloveOn && on && balls.Count > 0)
        {
            glove.Pose(2);
            Preview(true, true);
            glove.palm.GetComponent<AimBehaviour>().Cast(balls[selectedBall]);
        }
        else
        {
            Preview(true, false);
            glove.palm.GetComponent<AimBehaviour>().Clear();
        }
    }

    public void SwapBall(bool right)
    {
        if (gloveOn && balls.Count > 0)
        {
            AudioManager.instance.PlaySound(swapClip, glove.audioSource);

            if (right) selectedBall++;
            else selectedBall--;

            if (selectedBall < 0) selectedBall = balls.Count - 1;
            else if (selectedBall >= balls.Count) selectedBall = 0;

            glove.projection.GetComponent<ProjectionBehaviour>().UpdateBall(balls[selectedBall], gloveOn);
        }
    }

    public void ClearAuxiliars()
    {
        if (gloveOn)
        {
            AudioManager.instance.PlaySound(clearClip, glove.audioSource);

            foreach (GameObject ball in auxiliarBalls) Destroy(ball);
            auxiliarBalls.Clear();
        }
    }

    public void ObtainBall(Ball ball)
    {
        balls.Add(ball);
        selectedBall = balls.Count - 1;
        glove.projection.GetComponent<ProjectionBehaviour>().UpdateBall(balls[selectedBall], gloveOn);
    }

    public void ClearBalls()
    {
        balls.Clear();
        glove.projection.GetComponent<ProjectionBehaviour>().UpdateBall(null, false);
    }

    public void Preview(bool on, bool isPalm)
    {
        if (gloveOn &&balls.Count > 0)
        {
            if (on)
            {
                if (isPalm)
                {
                    glove.projection.transform.position = glove.palm.position;
                    glove.projection.transform.rotation = glove.palm.rotation;
                    glove.projection.transform.localScale = glove.palm.localScale;
                }
                else
                {
                    glove.projection.transform.position = glove.wrist.position;
                    glove.projection.transform.rotation = glove.wrist.rotation;
                    glove.projection.transform.localScale = glove.wrist.localScale;
                }
                ShowProjection(true);
            }
            else ShowProjection(false);
        }
    }

    void SelectBall(int idx)
    {
        selectedBall = idx;
        glove.projection.GetComponent<ProjectionBehaviour>().UpdateBall(balls[selectedBall], gloveOn);
    }

    IEnumerator Shot()
    {
        Preview(false, false);
        glove.Pose(2);
        shot = true;

        GameObject ball = Instantiate(balls[selectedBall].prefab);

        if (balls[selectedBall].ballType != Ball.BallType.ItemSpawn)
        {
            // Set ball
            if (ball.GetComponent<BallBehaviour>()) ball.GetComponent<BallBehaviour>().ball = Instantiate(balls[selectedBall]);
        }

        // If ball is auxiliar, track it
        if (balls[selectedBall].auxiliar) auxiliarBalls.Add(ball);

        // Set position (Palm)
        ball.transform.position = glove.palm.position;

        // Set rotation (Palm or Gyroscope)
        ball.transform.rotation = glove.palm.rotation;

        yield return new WaitForSeconds(1f);

        shot = false;
        glove.Pose(0);
        Preview(true, true);
    }

    IEnumerator CallPause()
    {
        if (!GameManager.Instance.paused)
        {
            yield return new WaitForSeconds(0.5f);

            if (InputManager.Instance.Holding(InputManager.Instance.pause))
            {
                GameManager.Instance.Pause(true);
                switch (gameMode)
                {
                    case GameMode.Hands:
                    case GameMode.Controllers:
                        canvasVRInstance = Instantiate(canvasVR);
                        canvasVRInstance.transform.position = canvasVRPos.position;
                        canvasVRInstance.transform.rotation = Quaternion.Euler(canvasVRPos.rotation.eulerAngles.x, canvasVRPos.rotation.eulerAngles.y, 0);
                        break;
                    case GameMode.Desktop:
                        D_pauseMenu.SetActive(true);
                        break;
                }
            }
        }
        else
        {
            GameManager.Instance.Pause(false);
            Destroy(canvasVRInstance);
            D_pauseMenu.SetActive(false);
        }
    }

    #region Hands exclusive methods

    public void ShowProjection(bool on)
    {
        glove.projection.SetActive(on);
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
            SwapBall(true);
            Preview(true, false);
        }
    }

    #endregion

    #region Desktop exclusive methods

    void FixedUpdate()
    {
        if (gameMode == GameMode.Desktop)
        {
            D_body.linearVelocity =
                speed * velForward * D_body.transform.forward +
                speed * velSide * D_body.transform.right +
                D_body.linearVelocity.y * D_body.transform.up;
        }
    }

    #endregion
}
