using UnityEngine;
using static Glossary;

public class PalmRegionBehaviour : MonoBehaviour
{
    public bool entered = false;
    int timerMax = 100;
    int timer = 0;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(GetTag(Tag.LeftHand)))
        {
            entered = true;
            timer = timerMax;
        }
    }

    void FixedUpdate()
    {
        if (timer > 0)
        {
            timer--;
            if (timer <= 0) entered = false;
        }
    }
}
