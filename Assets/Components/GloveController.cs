using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloveController : MonoBehaviour
{
    [Header("The ball")]
    [SerializeField] GameObject ballPrefab;
    [SerializeField] List<Ball> balls = new();

    [Header("Preview & calculations")]
    [SerializeField] GameObject projection;
    [SerializeField] Transform wrist;
    [SerializeField] Transform palm;

    AimBeam aimBeam;
    int selectedBall = 0;
    bool aiming = false;
    bool shot = false;

    void Start()
    {
        projection.SetActive(false);
        projection.GetComponent<ProjectionBehaviour>().UpdateBall(balls[selectedBall]);

        aimBeam = palm.GetComponent<AimBeam>();
    }

    void Update()
    {
        if (aiming) aimBeam.Cast(balls[selectedBall]);
        else aimBeam.Clear();
    }

    public void ObtainBall(Ball ball)
    {
        balls.Add(ball);
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

                projection.SetActive(true);
            }
            else projection.SetActive(false);
        }
    }

    public void SwitchBall(bool right)
    {
        Debug.Log("do");
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

    public void SwitchBallNew()
    {
        selectedBall = balls.Count - 1;
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

        GameObject ball = Instantiate(ballPrefab);

        ball.GetComponent<BallBehaviour>().ball = balls[selectedBall];

        ball.transform.position = palm.position;
        ball.transform.rotation = palm.rotation;

        yield return new WaitForSeconds(1f);

        shot = false;
        Preview(true, true);
    }
}
