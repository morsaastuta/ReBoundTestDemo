using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    public Ball ball;

    Rigidbody body;

    bool collided = false;

    Vector3 lastPoint = new();

    void Start()
    {
        body = GetComponent<Rigidbody>();

        GetComponent<MeshFilter>().sharedMesh = ball.mesh;
        GetComponent<MeshRenderer>().material = ball.material;
        if (ball.ballType.Equals(Ball.BallType.Object)) GetComponent<MeshCollider>().sharedMesh = ball.mesh;

        lastPoint = transform.position;
    }

    void FixedUpdate()
    {
        if (!ball.stopsOnContact || (ball.stopsOnContact && !collided)) body.linearVelocity = transform.forward * ball.TranslationSpeed();
        else body.linearVelocity = new Vector3(0, 0, 0);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Is the collision reboundable?
        if ((ball.reboundInfinitely || ball.reboundCount >= 0) && collision.collider.CompareTag(Glossary.GetTag(Glossary.Tag.Reboundable)))
        {
            collided = true;

            switch (ball.ballType)
            {
                case Ball.BallType.Sphere: ReboundSphere(collision.collider.transform, collision.GetContact(0)); break;
            }

            lastPoint = transform.position;
            ball.Rebound();
        }
        else Destroy(gameObject);
    }

    void ReboundSphere(Transform plane, ContactPoint contact)
    {
        // TRANSFORM FORWARD (+Z) IS THE FORWARD DIRECTION OF THE BALL
        // TRANSFORM RIGHT (+X) IS AXIS X
        // TRANSFORM UP (+Y) IS AXIS Y
        // TRANSFORM ROT Y IS THE HORIZONTAL ROTATION OF THE BALL
        // TRANSFORM ROT X IS THE VERTICAL ROTATION OF THE BALL

        // Debug of impact
        // Debug.DrawRay(contact.point, plane.up, Color.red, 99f);
        // Debug.DrawRay(contact.point, plane.right, Color.red, 99f);
        // Debug.DrawLine(transform.position, lastPoint, Color.green, 99f);

        // Calculation of horizontal and vertical amplitudes to decide if ball goes right/left or up/down
        float xAmplitude = Vector3.Angle((lastPoint - transform.position).normalized, plane.right.normalized);
        float yAmplitude = Vector3.Angle((lastPoint - transform.position).normalized, plane.up.normalized);

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
}
