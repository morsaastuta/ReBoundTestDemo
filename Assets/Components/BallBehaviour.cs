using Oculus.Interaction;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    [SerializeField] Ball ball;

    bool collided = false;

    void Start()
    {
        GetComponent<MeshFilter>().sharedMesh = ball.mesh;
        GetComponent<MeshCollider>().sharedMesh = ball.mesh;
    }

    void FixedUpdate()
    {
        if (!ball.stopsOnContact || (ball.stopsOnContact && !collided)) transform.position += Vector3.forward * ball.TranslationSpeed();
    }

    void OnCollisionEnter(Collision collision)
    {
        collided = true;

        // Can ball still rebound?
        if (ball.reboundInfinitely || ball.reboundCount >= 0)
        {
            // Is the collision reboundable?
            if (collision.collider.CompareTag(Glossary.GetTag(Glossary.Tag.Reboundable))) Rebound(collision.collider);
        }
        else Destroy(gameObject);
    }

    void Rebound(Collider collider)
    {
        ball.Rebound();
        transform.rotation = Quaternion.LookRotation(transform.position - collider.transform.position) * Quaternion.Euler(0, ball.reboundAngles, 0);
    }
}