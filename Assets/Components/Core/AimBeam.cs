using UnityEngine;
using System.Collections.Generic;

public class AimBeam : MonoBehaviour
{
    LineRenderer beam;
    List<Vector3> beamIndices = new();

    void Start()
    {
        beam = GetComponent<LineRenderer>();
    }

    void AnullingBeam()
    {
        SetBeam(.02f, .02f, Color.red, Color.red);
    }

    void ReboundBeam()
    {
        SetBeam(.02f, .02f, Color.green, Color.yellow);
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
        beamIndices.Clear();
        beamIndices.Add(transform.position);
        Ray ray = new(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, ball.linearSpeed))
        {
            beamIndices.Add(hit.point);

            if (ball.reboundAngles != 0 && hit.collider.CompareTag(Glossary.GetTag(Glossary.Tag.Reboundable)))
            {
                ReboundBeam();
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

                beamIndices.Add(hit.point + reboundDirection * Vector3.forward * ball.linearSpeed);
            }
        }
        else
        {
            beamIndices.Add(ray.GetPoint(ball.linearSpeed));
        }
    }

    void Update()
    {
        beam.positionCount = beamIndices.Count;

        foreach (Vector3 index in beamIndices) beam.SetPosition(beamIndices.IndexOf(index), index);
    }
}
