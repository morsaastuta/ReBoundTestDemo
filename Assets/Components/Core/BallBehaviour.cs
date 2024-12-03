using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    public Ball ball;
    Rigidbody body;
    Vector3 lastVertex = new();
    List<Collider> internalColliders = new();

    void Start()
    {
        // Set rigidbody
        body = GetComponent<Rigidbody>();

        // Set internal colliders to prevent fake rebounds
        internalColliders.AddRange(GetComponentsInChildren<Collider>());

        // Set mesh & material
        GetComponent<MeshFilter>().sharedMesh = ball.mesh;
        GetComponent<MeshRenderer>().material = ball.material;
        if (ball.ballType.Equals(Ball.BallType.Object)) GetComponent<MeshCollider>().sharedMesh = ball.mesh;

        // Set last rebound position as initial position
        lastVertex = transform.position;
    }

    void FixedUpdate()
    {
        // Keep moving ball while it is NOT sticky or while it IS sticky but it has NOT collided still
        if (!ball.sticky || (ball.sticky && ball.reboundCount == 0)) body.linearVelocity = transform.forward * ball.TranslationSpeed();
        else body.linearVelocity = new Vector3(0, 0, 0);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Prevent ball's internal collisions counting as rebounds
        if (internalColliders.Contains(collision.collider)) return;

        // Is the collision reboundable?
        if (collision.collider.CompareTag(Glossary.GetTag(Glossary.Tag.Reboundable)) && (ball.reboundInfinitely || ball.reboundCount <= ball.reboundLimit))
        {
            // If the ball is not sticky
            if (!ball.sticky)
            {
                // Select rebound method according to the ball type
                switch (ball.ballType)
                {
                    case Ball.BallType.Sphere: ReboundSphere(collision.collider.transform, collision.GetContact(0)); break;
                    case Ball.BallType.Prism: ReboundPrism(collision.collider.transform, collision.GetContact(0)); break;
                    case Ball.BallType.Object: ReboundObject(collision.collider.transform, collision.GetContact(0)); break;
                }

                // Update last rebound position
                lastVertex = transform.position;
            }
            // If the ball is a gyroscope, adopt the first collider's rotation forever
            else if (ball.gyroscope) transform.rotation = collision.collider.transform.rotation;

            // Update ball's properties on rebound
            ball.Rebound();
        }
        else if (!ball.persistent) Destroy(gameObject);

    }

    void ReboundSphere(Transform plane, ContactPoint contact)
    {
        // TRANSFORM FORWARD (+Z) IS THE FORWARD DIRECTION OF THE BALL
        // TRANSFORM RIGHT (+X) IS AXIS X
        // TRANSFORM UP (+Y) IS AXIS Y
        // TRANSFORM ROT Y IS THE HORIZONTAL ROTATION OF THE BALL
        // TRANSFORM ROT X IS THE VERTICAL ROTATION OF THE BALL

        // Debug of impact
        Debug.DrawRay(contact.point, plane.up, Color.red, 99f);
        Debug.DrawRay(contact.point, plane.right, Color.red, 99f);
        Debug.DrawLine(transform.position, lastVertex, Color.green, 99f);

        // Calculation of horizontal and vertical amplitudes to decide if ball goes right/left or up/down
        float xAmplitude = Vector3.Angle((lastVertex - transform.position).normalized, plane.right.normalized);
        float yAmplitude = Vector3.Angle((lastVertex - transform.position).normalized, plane.up.normalized);

        float xProximity = xAmplitude;
        if (xProximity > 90) xProximity = 180 - xAmplitude;

        float yProximity = yAmplitude;
        if (yProximity > 90) yProximity = 180 - yAmplitude;

        // Before rotating the ball, set rotation of plane
        transform.rotation = plane.rotation;

        if (xProximity < yProximity)
        {
            // If rebound is horizontal, rotate from Y axis
            if (xAmplitude < 90) transform.rotation *= Quaternion.Euler(0, -90 + ball.reboundAngles, 0);
            else transform.rotation *= Quaternion.Euler(0, 90 - ball.reboundAngles, 0);
        }
        else
        {
            // If rebound is vertical, rotate from X axis
            if (yAmplitude > 90) transform.rotation *= Quaternion.Euler(-90 + ball.reboundAngles, 0, 0);
            else transform.rotation *= Quaternion.Euler(90 - ball.reboundAngles, 0, 0);
        }
    }

    void ReboundPrism(Transform plane, ContactPoint contact)
    {
    }

    void ReboundObject(Transform plane, ContactPoint contact)
    {
    }
}
