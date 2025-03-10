using UnityEngine;

public class ProjectionBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Mesh emptyMesh;

    public Ball ball;

    void Start()
    {
        UpdateVisuals(false);
    }

    void FixedUpdate()
    {
        if (ball != null) transform.rotation *= Quaternion.Euler(0, ball.angularSpeed, 0);
    }

    public void UpdateVisuals(bool gloveOn)
    {
        if (gloveOn) GetComponent<MeshFilter>().sharedMesh = ball.mesh;
        else GetComponent<MeshFilter>().sharedMesh = emptyMesh;
    }

    public void UpdateBall(Ball b, bool gloveOn)
    {
        ball = b;
        UpdateVisuals(gloveOn);
    }
}
