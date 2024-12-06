using NUnit.Framework;
using UnityEngine;
using static Glossary;

public class ConditionalReboundBehaviour : MonoBehaviour
{
    [Header("Customization")]
    [SerializeField] public bool colorCondition;
    [SerializeField] public CustomColor color;
    [SerializeField] public bool countCondition;
    [SerializeField] public int count;

    public int conditionQty = 0;

    void Start()
    {
        SetConditionQty();
    }

    public void SetConditionQty()
    {
        conditionQty = 0;
        if (colorCondition) conditionQty++;
        if (countCondition) conditionQty++;
    }
}
