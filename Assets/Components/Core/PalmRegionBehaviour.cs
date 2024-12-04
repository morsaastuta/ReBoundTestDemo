using UnityEngine;

public class PalmRegionBehaviour : MonoBehaviour
{
    public bool entered = false;
    int timerMax = 100;
    int timer = 0;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(Glossary.GetTag(Glossary.Tag.LeftHand)))
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
