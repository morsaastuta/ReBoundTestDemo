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

    void OnTriggerStay(Collider collider)
    {
        if (LayerMask.LayerToName(collider.gameObject.layer).Equals(GetLayer(Layer.Ball)))
        {
            // If colliding with a ball of different color, destroy it
            if (collider.GetComponent<MeshRenderer>().material.color != GetColor(color)) Destroy(collider.gameObject);
        }
    }
}
