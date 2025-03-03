using DG.Tweening;
using UnityEngine;
using static Glossary;
using static Unity.VisualScripting.Member;

public abstract class ActivatorBehaviour : MonoBehaviour
{
    [Header("Customization (Activator)")]
    [SerializeField] public bool active = false;
    [SerializeField] protected float timerMax;

    protected float timer = 0;

    protected void FixedUpdate()
    {
        if (timer > 0)
        {
            timer--;
            if (timer <= 0) Activate(false);
        }
    }

    protected virtual void Activate(bool on)
    {
        active = on;

        if (on)
        {
            if (timerMax > 0) timer = timerMax;
        }
        else
        {

        }
    }
}