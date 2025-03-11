using UnityEngine;
using System.Collections.Generic;
using static Glossary;

public class AimBehaviour : MonoBehaviour
{
    [SerializeField] LineRenderer beam;
    List<Vector3> beamIndices = new();

    void Start()
    {
        SetAnnulling();
    }

    void SetAnnulling()
    {
        SetBeam(.015f, .015f, Color.red, Color.red);
    }

    void SetReboundable()
    {
        SetBeam(.015f, .015f, Color.green, Color.yellow);
    }

    public void SetBeam(float sw, float ew, Color sc, Color ec)
    {
        beam.startWidth = sw;
        beam.endWidth = ew;
        beam.startColor = sc;
        beam.endColor = ec;
    }

    public void Clear()
    {
        beam.positionCount = 0;
        beamIndices.Clear();
    }

    public void Cast(Ball ball)
    {
        // Reboundable by default
        SetReboundable();

        beamIndices.Clear();
        beamIndices.Add(transform.position);
        Ray ray = new(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, ball.linearSpeed * 3))
        {
            beamIndices.Add(hit.point);

            // Regular calculation if collider is reboundable
            if (hit.collider.CompareTag(GetTag(Tag.Reboundable)))
            {
                // Cancel if auxiliar/interactable collision
                if (ball.auxiliar && hit.collider.gameObject.layer == LayerMask.NameToLayer(GetLayer(Layer.Interactable)))
                {
                    SetAnnulling();
                    return;
                }

                // Only if the ball can rebound, calculate rebound trajectory
                if (ball.reboundLimit != 0)
                {
                    // Copy of the code in BallBehaviour's SphereRebound()

                    float xAmplitude = Vector3.Angle((transform.position - hit.point).normalized, hit.transform.right.normalized);
                    float yAmplitude = Vector3.Angle((transform.position - hit.point).normalized, hit.transform.up.normalized);

                    float xProximity = xAmplitude;
                    if (xProximity > 90) xProximity = 180 - xAmplitude;

                    float yProximity = yAmplitude;
                    if (yProximity > 90) yProximity = 180 - yAmplitude;

                    Quaternion reboundDirection = hit.transform.rotation;

                    if (xProximity < yProximity)
                    {
                        // If rebound is horizontal, rotate from Y axis
                        if (xAmplitude < 90) reboundDirection *= Quaternion.Euler(0, -90 + ball.reboundAngles, 0);
                        else reboundDirection *= Quaternion.Euler(0, 90 - ball.reboundAngles, 0);
                    }
                    else
                    {
                        // If rebound is vertical, rotate from X axis
                        if (yAmplitude > 90) reboundDirection *= Quaternion.Euler(-90 + ball.reboundAngles, 0, 0);
                        else reboundDirection *= Quaternion.Euler(90 - ball.reboundAngles, 0, 0);
                    }

                    // Continuation of the beam
                    Ray rbRay = new(hit.point, hit.transform.forward);
                    if (Physics.Raycast(rbRay, out RaycastHit rbHit, ball.linearSpeed)) beamIndices.Add(rbHit.point);
                    else beamIndices.Add(hit.point + reboundDirection * Vector3.forward * ball.linearSpeed);
                }
            }
            // If collision exists but it's not reboundable, change beam color and don't add length
            else SetAnnulling();
        }
        else beamIndices.Add(ray.GetPoint(ball.linearSpeed));
    }

    void Update()
    {
        beam.positionCount = beamIndices.Count;

        foreach (Vector3 index in beamIndices) beam.SetPosition(beamIndices.IndexOf(index), index);
    }
}
