using System.Collections.Generic;
using UnityEngine;
using static Glossary;

[CreateAssetMenu(fileName = "Ball", menuName = "Scriptable Objects/Ball")]
public class Ball : ScriptableObject
{
    public enum BallType
    {
        Sphere, Prism, Object, ItemSpawn
    }

    [Header("Identity")]
    [SerializeField] public string title;
    [SerializeField] public string description;
    [SerializeField] public BallType ballType;
    [SerializeField] public GameObject prefab;

    [Header("Projection")]
    [SerializeField] public Mesh mesh;
    [SerializeField] public Material material;

    [Header("Rebound")]
    [SerializeField] public float reboundAngles;
    [SerializeField] public int reboundCount;
    [SerializeField] public int reboundLimit;
    [SerializeField] public bool reboundInfinitely = false;

    [Header("Auxiliar")]
    [SerializeField] public bool auxiliar = false;
    [SerializeField] public bool sticky = false;
    [SerializeField] public bool gyroscope = false;
    [SerializeField] public bool persistent = false;

    [Header("Traslation")]
    [SerializeField] public bool translates = true;
    [SerializeField] public float linearSpeed;
    [SerializeField] public float linearFriction;

    [Header("Curve")]
    [SerializeField] public bool curves = false;
    [SerializeField] public float angularSpeed;
    [SerializeField] public float angularFriction;

    [Header("Color")]
    [SerializeField] public List<CustomColor> colors = new();

    public void Rebound()
    {
        if (reboundCount <= reboundLimit) reboundCount++;
    }

    public float TranslationSpeed()
    {
        if (translates)
        {
            float value = linearSpeed + linearFriction * reboundLimit;

            if (value < 0) return 0;

            return value;
        }
        else return 0;
    }

    public float CurveSpeed()
    {
        if (curves)
        {
            float value = angularSpeed + angularFriction * reboundLimit;

            if (value < 0) return 0;

            return value;
        }
        else return 0;
    }
}
