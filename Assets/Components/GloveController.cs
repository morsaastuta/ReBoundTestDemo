using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloveController : MonoBehaviour
{
    [SerializeField] Transform projectionPos;
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject ballPrefab;

    GameObject projection;

    [SerializeField] List<Ball> balls = new();
    int selectedBall = 0;
    bool shot = false;

    void Update()
    {
        if (!shot && InputManager.instance.Pressed(InputManager.instance.shoot)) StartCoroutine(Shoot());

        if (InputManager.instance.Pressed(InputManager.instance.switchL)) SwitchBall(false);

        if (InputManager.instance.Pressed(InputManager.instance.switchR)) SwitchBall(true);

        UpdateVisuals();
    }

    void SwitchBall(bool right)
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

    public void UpdateVisuals()
    {
        projection.SetActive(false);

        projection = null;

        if (balls.Count > 0) projection.GetComponent<MeshFilter>().sharedMesh = balls[selectedBall].mesh;

        if (projection != null) projection.SetActive(false);
    }

    IEnumerator Shoot()
    {
        shot = true;

        projection.SetActive(false);

        GameObject ball = Instantiate(ballPrefab);

        ball.transform.position = OVRControllerManager.instance.RTouchPosition;

        yield return new WaitForSeconds(1f);

        shot = false;
    }
}
