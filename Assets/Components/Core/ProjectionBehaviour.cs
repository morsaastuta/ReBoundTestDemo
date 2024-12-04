using UnityEngine;

public class ProjectionBehaviour : MonoBehaviour
{
    public Ball ball;

    void Start()
    {
        UpdateVisuals();
    }

    void FixedUpdate()
    {
        transform.rotation *= Quaternion.Euler(0, ball.angularSpeed, 0);
    }

    public void UpdateVisuals()
    {
        GetComponent<MeshFilter>().sharedMesh = ball.mesh;
        GetComponent<MeshRenderer>().material = ball.material;
    }

    public void UpdateBall(Ball b)
    {
        ball = b;
        UpdateVisuals();
    }
}
