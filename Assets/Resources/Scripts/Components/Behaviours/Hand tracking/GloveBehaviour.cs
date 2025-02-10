using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloveBehaviour : MonoBehaviour
{
    [Header("Preview & calculations")]
    [SerializeField] GameObject projection;
    [SerializeField] Transform eyes;
    [SerializeField] Transform wrist;
    [SerializeField] Transform palm;
    [SerializeField] PalmRegionBehaviour palmRegion;
    [SerializeField] OVRHand hand;

    [Header("Audio")]
    [SerializeField] AudioSource source;

    [Header("Debug")]
    [SerializeField] List<Ball> demoBalls = new();

    public AimBeam aimBeam;
    BallSelectorController ballSelector;
    int selectedBall = 0;
    bool aiming = false;
    bool shot = false;
    List<Ball> balls = new();
    List<GameObject> auxiliarBalls = new();

    void Start()
    {
        // Get aim beam
        aimBeam = palm.GetComponent<AimBeam>();

        // Add demo balls
        foreach (Ball ball in demoBalls) ObtainBall(ball);

        // Set default projection using first ball if it exists
        if (balls.Count < 0) projection.GetComponent<ProjectionBehaviour>().UpdateBall(balls[selectedBall]);
    }

    void Update()
    {
        // While aiming...
        if (aiming)
        {
            // If no ball was recently shot...
            if (!shot)
            {
                // Update the aim beam rendering
                if (Vector3.Angle(eyes.forward, palm.forward) <= 90) aimBeam.Cast(balls[selectedBall]);
                else aimBeam.Clear();

                // Pinch index finger to shoot ball
                if (hand.GetFingerIsPinching(OVRHand.HandFinger.Index)) Shoot();
            }
            else aimBeam.Clear();

            // Pinch middle finger to delete all auxiliar balls
            if (hand.GetFingerIsPinching(OVRHand.HandFinger.Middle)) ClearAuxiliars();
        }
        else aimBeam.Clear();
    }

    public void Shoot()
    {
        shot = true;
        StartCoroutine(Shot());
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

    public void ShowProjection(bool on)
    {
        projection.SetActive(on);
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

    public void SelectBall(int idx)
    {
        selectedBall = idx;
        projection.GetComponent<ProjectionBehaviour>().UpdateBall(balls[selectedBall]);
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

    public IEnumerator Shot()
    {
        Preview(false, false);

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
}
