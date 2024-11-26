using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloveController : MonoBehaviour
{
    [SerializeField] Transform projectionPos;
    GameObject projection;

    List<GameObject> balls = new();
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

    public void GetBall(GameObject ball)
    {
        balls.Add(ball);
    }

    public void UpdateVisuals()
    {
        projection.SetActive(false);

        projection = null;

        if (balls.Count > 0) projection = balls[selectedBall];

        if (projection != null) projection.SetActive(false);
    }

    IEnumerator Shoot()
    {
        shot = true;

        GameObject ball = Instantiate(balls[selectedBall]);

        ball.transform.position = OVRControllerManager.instance.RTouchPosition;

        yield return new WaitForSeconds(1f);

        shot = false;
    }
}
