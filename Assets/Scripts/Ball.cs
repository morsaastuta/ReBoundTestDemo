using UnityEngine;

[CreateAssetMenu(fileName = "Ball", menuName = "Scriptable Objects/Ball")]
public class Ball : ScriptableObject
{
    [Header("Identity")]
    public string title;
    public Mesh mesh;
    public Material material;

    [Header("Rebound")]
    public float reboundAngles;
    public int reboundCount;
    public int reboundCounter;
    public bool reboundInfinitely = false;
    public bool stopsOnContact = false;

    [Header("Traslation")]
    public bool translates = true;
    public float linearSpeed;
    public float linearFriction;

    [Header("Curve")]
    public bool curves = false;
    public float angularSpeed;
    public float angularFriction;

    public void Rebound()
    {
        if (reboundCounter >= 0)
        {
            reboundCounter--;
            reboundCount++;
        }
    }

    public float TranslationSpeed()
    {
        if (translates)
        {
            float value = linearSpeed + linearFriction * reboundCounter;

            if (value < 0) return 0;

            return value;
        }
        else return 0;
    }

    public float CurveSpeed()
    {
        if (curves)
        {
            float value = angularSpeed + angularFriction * reboundCounter;

            if (value < 0) return 0;

            return value;
        }
        else return 0;
    }
}
