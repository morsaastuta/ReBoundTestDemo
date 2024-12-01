using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.PackageManager;
using UnityEngine;

public class GloveController : MonoBehaviour
{
    [SerializeField] GameObject projection;
    [SerializeField] Transform shotOrigin;
    [SerializeField] AimBeam aimBeam;
    [SerializeField] GameObject ballPrefab;

    [SerializeField] List<Ball> balls = new();

    int selectedBall = 0;
    bool aiming = false;
    bool shot = false;

    void Start()
    {
        projection.SetActive(false);
    }

    void Update()
    {
        if (aiming) aimBeam.Cast(balls[selectedBall]);
        else aimBeam.Clear();
    }

    public void SwitchBall(bool right)
    {
        if (right) selectedBall++;
        else selectedBall--;

        if (selectedBall < 0) selectedBall = balls.Count - 1;
        else if (selectedBall >= balls.Count) selectedBall = 0;
    }

    public void ObtainBall(Ball ball)
    {
        balls.Add(ball);
    }

    public void ShowProjection()
    {
        if (balls.Count > 0)
        {
            projection.SetActive(true);
            projection.GetComponent<ProjectionBehaviour>().UpdateBall(balls[selectedBall]);
        }
    }

    public void HideProjection()
    {
        projection.SetActive(false);
    }

    public void Aim(bool on)
    {
        aiming = on;
        Debug.Log(on);
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
        HideProjection();

        GameObject ball = Instantiate(ballPrefab);

        ball.GetComponent<BallBehaviour>().ball = balls[selectedBall];

        ball.transform.position = shotOrigin.position;
        ball.transform.rotation = shotOrigin.rotation;

        yield return new WaitForSeconds(1f);

        shot = false;
    }
}
