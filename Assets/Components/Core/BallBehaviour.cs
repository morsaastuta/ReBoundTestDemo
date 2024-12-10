using System.Collections.Generic;
using UnityEngine;
using static Glossary;

public class BallBehaviour : MonoBehaviour
{
    [SerializeField] List<GameObject> expansion = new();
    [Header("Audio")]
    [SerializeField] AudioClip reboundClip;
    [SerializeField] AudioClip anullClip;
    public Ball ball;
    Rigidbody body;
    Vector3 lastVertex = new();
    List<Collider> bypassedColliders = new();
    AudioSource source;

    void Start()
    {
        // Deactivate expansion modules
        foreach (GameObject go in expansion) go.SetActive(false);

        // Set rigidbody
        body = GetComponent<Rigidbody>();

        // Bypass internal colliders to prevent fake rebounds
        bypassedColliders.AddRange(GetComponentsInChildren<Collider>());

        // Set mesh & material
        GetComponent<MeshFilter>().sharedMesh = ball.mesh;
        GetComponent<MeshRenderer>().material = ball.material;
        if (ball.ballType.Equals(Ball.BallType.Object)) GetComponent<MeshCollider>().sharedMesh = ball.mesh;

        // Set last rebound position as initial position
        lastVertex = transform.position;

        // Set greenToRed colors
        if (ball.colors.Count > 0) GetComponent<MeshRenderer>().material.color = GetColor(ball.colors[0]);

        // Set audio source
        source = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        // Keep moving ball while it is NOT sticky or while it IS sticky but it has NOT collided still
        if (!ball.sticky || (ball.sticky && ball.reboundCount == 0)) body.linearVelocity = transform.forward * ball.TranslationSpeed();
        else body.linearVelocity = new Vector3(0, 0, 0);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (Prebound(collision.collider))
        {
            source.clip = anullClip;
            source.Play();
            return;
        }

        source.clip = reboundClip;
        source.Play();
        Rebound(collision.collider);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (Prebound(collider)) return;

        // Inside BALL BEHAVIOUR, ON TRIGGER ENTER will only be used for conditional collisions (e.g. Colored Bounds)
        if (collider.GetComponent<ConditionalReboundBehaviour>())
        {
            // Condition check (FALSE -> pass through // TRUE -> rebound)
            ConditionalReboundBehaviour crb = collider.GetComponent<ConditionalReboundBehaviour>();
            int checkQty = 0;
            if (crb.colorCondition && GetColor(crb.color) == GetComponent<MeshRenderer>().material.color) checkQty++;
            if (crb.countCondition && crb.count == ball.reboundCount) checkQty++;
            if (checkQty >= crb.conditionQty) return;

            Rebound(collider);
        }
    }

    bool Prebound(Collider collider)
    {
        // Bypass certain colliders
        if (bypassedColliders.Contains(collider)) return true;

        // Annulling colliders destroy balls
        if (collider.CompareTag(GetTag(Tag.Annulling)))
        {
            Destroy(gameObject);
            return true;
        }

        return false;
    }

    void Rebound(Collider collider)
    {
        // Is the ball auxiliar?
        if (ball.auxiliar)
        {
            // Destroy auxiliar objects if they make contact with interactables
            if (LayerMask.LayerToName(collider.gameObject.layer).Equals(GetLayer(Layer.Interactable)))
            {
                Destroy(gameObject);
                return;
            }
        }

        // Is the collider reboundable? Can the ball keep rebounding?
        if (collider.CompareTag(GetTag(Tag.Reboundable)) && (ball.reboundInfinitely || ball.reboundCount < ball.reboundLimit))
        {
            // If the ball is not sticky
            if (!ball.sticky)
            {
                // Select rebound method according to the ball type
                switch (ball.ballType)
                {
                    case Ball.BallType.Sphere: ReboundSphere(collider.transform); break;
                    case Ball.BallType.Prism: ReboundPrism(collider.transform); break;
                    case Ball.BallType.Object: ReboundObject(collider.transform); break;
                }

                // Update last rebound position
                lastVertex = transform.position;
            }
            // If the ball is a gyroscope, adopt the first collider's rotation forever
            else if (ball.gyroscope) transform.rotation = collider.transform.rotation;

            // Update ball's properties on rebound
            ball.Rebound();

            // Expand
            foreach (GameObject go in expansion) go.SetActive(true);

            // Set greenToRed colors
            if (ball.reboundCount < ball.colors.Count) GetComponent<MeshRenderer>().material.color = GetColor(ball.colors[ball.reboundCount]);
        }
        else if (!ball.persistent) Destroy(gameObject);
    }

    void ReboundSphere(Transform plane)
    {
        // TRANSFORM FORWARD (+Z) IS THE FORWARD DIRECTION OF THE BALL
        // TRANSFORM RIGHT (+X) IS AXIS X
        // TRANSFORM UP (+Y) IS AXIS Y
        // TRANSFORM ROT Y IS THE HORIZONTAL ROTATION OF THE BALL
        // TRANSFORM ROT X IS THE VERTICAL ROTATION OF THE BALL

        Debug.DrawLine(plane.position, plane.position + plane.right * 100, Color.red, 99);
        Debug.DrawLine(plane.position, plane.position + plane.up * 100, Color.green, 99);
        Debug.DrawLine(plane.position, plane.position + plane.forward * 100, Color.blue, 99);

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

    void ReboundPrism(Transform plane)
    {
    }

    void ReboundObject(Transform plane)
    {
    }
}
