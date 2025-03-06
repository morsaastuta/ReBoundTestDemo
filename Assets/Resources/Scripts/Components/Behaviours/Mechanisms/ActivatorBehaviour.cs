using UnityEngine;
using UnityEngine.Events;

public abstract class ActivatorBehaviour : MonoBehaviour
{
    [Header("Customization (Activator)")]
    [SerializeField] public bool active = false;
    [SerializeField] protected float timerMax;
    [SerializeField] protected UnityEvent call;

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
            if (call != null) call.Invoke();
            if (timerMax > 0) timer = timerMax;
        }
        else
        {

        }
    }
}