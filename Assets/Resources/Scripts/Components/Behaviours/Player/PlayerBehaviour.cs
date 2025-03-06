using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    Glove glove;

    [Header("VR references")]
    [SerializeField] GameObject canvasVR;
    [SerializeField] Transform canvasVRPos;

    // VR commons
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
    [SerializeField] Glove HR_glove;
    [SerializeField] Glove HL_glove;

    [Header("Controllers references")]
    [SerializeField] List<GameObject> controllersExclusives = new();
    [SerializeField] Glove CR_glove;
    [SerializeField] Glove CL_glove;
    [SerializeField] GameObject C_grabInteractorL;
    [SerializeField] GameObject C_distanceGrabInteractorL;
    [SerializeField] GameObject C_grabInteractorR;
    [SerializeField] GameObject C_distanceGrabInteractorR;

    [Header("Desktop references")]
    [SerializeField] List<GameObject> desktopExclusives = new();
    [SerializeField] Glove DR_glove;
    [SerializeField] Glove DL_glove;
    [SerializeField] Rigidbody D_body;
    [SerializeField] GameObject D_canvas;

    void Start()
    {
        SetGameMode(gameMode);
        SetHandedness(leftMode);
        UpdateComponents();

        // Add demo balls
        foreach (Ball ball in demoBalls) ObtainBall(ball);

        // Set default projection using first ball if it exists
        if (balls.Count < 0) glove.projection.GetComponent<ProjectionBehaviour>().UpdateBall(balls[selectedBall]);
    }

    public void SetGameMode(GameMode gm)
    {
        gameMode = gm;
        UpdateComponents();
    }

    public void SetHandedness(bool left)
    {
        leftMode = left;
        InputManager.instance.SetHandedness(leftMode);
        UpdateComponents();
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
                if (!leftMode) glove = CR_glove;
                else glove = CL_glove;
                break;

            case GameMode.Desktop:
                foreach (GameObject go in desktopExclusives) go.SetActive(true);
                if (!leftMode) glove = DR_glove;
                else glove = DL_glove;
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
                        if (Vector3.Angle(eyes.forward, glove.palm.forward) <= 90) glove.palm.GetComponent<AimBeam>().Cast(balls[selectedBall]);
                        else glove.palm.GetComponent<AimBeam>().Clear();

                        // Pinch index finger to shoot ball
                        if (glove.hand.GetFingerIsPinching(OVRHand.HandFinger.Index)) Shoot();
                    }
                    else glove.palm.GetComponent<AimBeam>().Clear();

                    // Pinch middle finger to delete all auxiliar balls
                    if (glove.hand.GetFingerIsPinching(OVRHand.HandFinger.Middle)) ClearAuxiliars();
                }
                else glove.palm.GetComponent<AimBeam>().Clear();

                break;

            // Desktop exclusive
            case GameMode.Desktop:

                Vector2 vMove = InputManager.instance.Inclination(InputManager.instance.move);
                velSide = vMove.x;
                velForward = vMove.y;

                Vector2 vLook = InputManager.instance.Inclination(InputManager.instance.look);
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + vLook.x, transform.rotation.eulerAngles.z);
                
                break;
        }

        if (InputManager.instance.Holding(InputManager.instance.shoot)) Shoot();

        if (InputManager.instance.Holding(InputManager.instance.aim)) glove.palm.GetComponent<AimBeam>().Cast(balls[selectedBall]);
        else glove.palm.GetComponent<AimBeam>().Clear();

        if (InputManager.instance.Pressed(InputManager.instance.swap)) SwapBall(true);

        if (InputManager.instance.Pressed(InputManager.instance.clear)) ClearAuxiliars();

        // Can't pause on MainMenu (ID 0)
        if (GameManager.instance.sceneID != 0 && InputManager.instance.Pressed(InputManager.instance.pause)) StartCoroutine(CallPause());
    }

    public void Shoot()
    {
        if (!shot)
        {
            AudioManager.instance.PlaySound(shootClip, glove.audioSource);
            StartCoroutine(Shot());
        }
    }

    public IEnumerator Shot()
    {
        Preview(false, false);
        shot = true;

        GameObject ball = Instantiate(balls[selectedBall].prefab);

        // Set ball
        if (ball.GetComponent<BallBehaviour>()) ball.GetComponent<BallBehaviour>().ball = Instantiate(balls[selectedBall]);
        
        // If ball is auxiliar, track it
        if (ball.GetComponent<BallBehaviour>().ball.auxiliar) auxiliarBalls.Add(ball);

        // Set position (Palm)
        ball.transform.position = glove.palm.position;

        // Set rotation (Palm or Gyroscope)
        ball.transform.rotation = glove.palm.rotation;

        yield return new WaitForSeconds(1f);

        shot = false;
        Preview(true, true);
    }

    public void SwapBall(bool right)
    {
        if (balls.Count > 0)
        {
            AudioManager.instance.PlaySound(swapClip, glove.audioSource);

            if (right) selectedBall++;
            else selectedBall--;

            if (selectedBall < 0) selectedBall = balls.Count - 1;
            else if (selectedBall >= balls.Count) selectedBall = 0;

            glove.projection.GetComponent<ProjectionBehaviour>().UpdateBall(balls[selectedBall]);
        }
    }

    public void ClearAuxiliars()
    {
        AudioManager.instance.PlaySound(clearClip, glove.audioSource);

        foreach (GameObject ball in auxiliarBalls) Destroy(ball);
        auxiliarBalls.Clear();
    }

    public void ObtainBall(Ball ball)
    {
        balls.Add(ball);
        selectedBall = balls.Count - 1;
        glove.projection.GetComponent<ProjectionBehaviour>().UpdateBall(balls[selectedBall]);
    }

    public void Preview(bool on, bool isPalm)
    {
        if (balls.Count > 0)
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

    public void SelectBall(int idx)
    {
        selectedBall = idx;
        glove.projection.GetComponent<ProjectionBehaviour>().UpdateBall(balls[selectedBall]);
    }

    public IEnumerator CallPause()
    {
        if (!GameManager.instance.paused)
        {
            yield return new WaitForSeconds(0.5f);

            if (InputManager.instance.Holding(InputManager.instance.pause))
            {
                GameManager.instance.Pause(true);
                switch (gameMode)
                {
                    case GameMode.Hands:
                    case GameMode.Controllers:
                        canvasVRInstance = Instantiate(canvasVR);
                        canvasVRInstance.transform.position = canvasVRPos.position;
                        canvasVRInstance.transform.rotation = Quaternion.Euler(canvasVRPos.rotation.eulerAngles.x, canvasVRPos.rotation.eulerAngles.y, 0);
                        break;
                    case GameMode.Desktop:
                        D_canvas.SetActive(true);
                        break;
                }
            }
        }
        else
        {
            GameManager.instance.Pause(false);
            Destroy(canvasVRInstance);
            D_canvas.SetActive(false);
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
            D_body.linearVelocity = new Vector3(speed * velSide, D_body.linearVelocity.y, speed * velForward);
        }
    }

    #endregion
}
