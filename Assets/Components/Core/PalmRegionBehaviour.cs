using UnityEngine;
using static Glossary;

public class PalmRegionBehaviour : MonoBehaviour
{
    [SerializeField] GloveBehaviour glove;
    bool inside = false;
    bool swiping = false;

    void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag(GetTag(Tag.LeftHand))) inside = true;
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag(GetTag(Tag.LeftHand))) inside = false;
    }

    public void SwipeInit()
    {
        swiping = true;
    }

    public void SwipeEnd()
    {
        if (swiping && inside) glove.SwitchBall(true);

        swiping = false;
    }
}
