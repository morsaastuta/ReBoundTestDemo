using UnityEngine;

public class ProjectionBehaviour : MonoBehaviour
{
    public Ball ball;

    void Start()
    {
        GetComponent<MeshFilter>().sharedMesh = ball.mesh;
        GetComponent<MeshRenderer>().material = ball.material;
    }

    void FixedUpdate()
    {
        transform.rotation = transform.rotation *= Quaternion.Euler(0, ball.angularSpeed, 0);
    }
}
