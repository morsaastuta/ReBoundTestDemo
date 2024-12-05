using UnityEngine;
using static Glossary;

public class ColoredBoundBehaviour : MonoBehaviour
{
    [Header("Customization")]
    [SerializeField] CustomColor color;

    void Start()
    {
        // Set color
        GetComponent<MeshRenderer>().material.color = new Color(GetColor(color).r, GetColor(color).g, GetColor(color).b, .5f);

        // Set conditions
        foreach (ConditionalReboundBehaviour crb in GetComponentsInChildren<ConditionalReboundBehaviour>())
        {
            crb.colorCondition = true;
            crb.color = color;
            crb.SetConditionQty();
        }
    }
}
