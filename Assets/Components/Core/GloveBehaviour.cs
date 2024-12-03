using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloveBehaviour : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] Transform eyes;
    [SerializeField] Transform canvas;

    [Header("Preview & calculations")]
    [SerializeField] GameObject projection;
    [SerializeField] Transform wrist;
    [SerializeField] Transform palm;

    [Header("Debug")]
    [SerializeField] List<Ball> demoBalls = new();

    AimBeam aimBeam;
    BallSelectorController ballSelector;
    int selectedBall = 0;
    bool aiming = false;
    bool shot = false;
    List<Ball> balls = new();

    void Start()
    {
        // Get aim beam
        aimBeam = palm.GetComponent<AimBeam>();

        // Get ball selector
        ballSelector = canvas.GetComponentInChildren<BallSelectorController>();

        // Turn off Canvas by default
        ShowCanvas(false);

        // Add demo balls
        foreach (Ball ball in demoBalls) ObtainBall(ball);

        // Set default projection using first ball if it exists
        if (balls.Count < 0) projection.GetComponent<ProjectionBehaviour>().UpdateBall(balls[selectedBall]);
    }

    void Update()
    {
        // Set canvas to always be towards the player
        canvas.LookAt(eyes);
        canvas.localRotation *= new Quaternion(0, 180, 0, 0);

        // Update the aim beam rendering
        if (aiming) aimBeam.Cast(balls[selectedBall]);
        else aimBeam.Clear();
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
                    ShowCanvas(false);
                }
                else
                {
                    projection.transform.position = wrist.position;
                    projection.transform.rotation = wrist.rotation;
                    projection.transform.localScale = wrist.localScale;
                    ShowCanvas(true);
                }
                ShowProjection(true);
            }
            else
            {
                ShowProjection(false);
                ShowCanvas(false);
            }
        }
    }

    void ShowProjection(bool on)
    {
        projection.SetActive(on);
    }

    void ShowCanvas(bool on)
    {
        if (on) foreach (Ball ball in balls) ballSelector.AddButton(ball, balls.IndexOf(ball));
        else ballSelector.ClearList();

        ballSelector.gameObject.SetActive(on);
    }

    public void SwitchBall(bool right)
    {
        Debug.Log("switch");
        if (projection.activeInHierarchy)
        {
            if (balls.Count > 0)
            {
                if (right) selectedBall++;
                else selectedBall--;

                if (selectedBall < 0) selectedBall = balls.Count - 1;
                else if (selectedBall >= balls.Count) selectedBall = 0;

                projection.GetComponent<ProjectionBehaviour>().UpdateBall(balls[selectedBall]);
            }
        }
    }

    public void SelectBall(int idx)
    {
        selectedBall = idx;
        projection.GetComponent<ProjectionBehaviour>().UpdateBall(balls[selectedBall]);
    }

    public void OpenPalm(bool on)
    {
        if (on) Preview(true, true);
        else Preview(true, false);
    }

    public void Aim(bool on)
    {
        aiming = on;
    }

    public void Shoot()
    {
        if (!shot)
        {
            shot = true;
            StartCoroutine(Shot());
        }
    }

    IEnumerator Shot()
    {
        Preview(false, false);

        GameObject ball = Instantiate(balls[selectedBall].prefab);

        // Set ball
        ball.GetComponent<BallBehaviour>().ball = Instantiate(balls[selectedBall]);

        // Set position (Palm)
        ball.transform.position = palm.position;

        // Set rotation (Palm or Gyroscope)
        ball.transform.rotation = palm.rotation;

        yield return new WaitForSeconds(1f);

        shot = false;
        Preview(true, true);
    }
}
