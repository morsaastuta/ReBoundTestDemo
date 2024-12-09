using DG.Tweening;
using UnityEngine;
using static Glossary;

public class ButtonBehaviour : MonoBehaviour
{
    [Header("Customization")]
    [SerializeField] float timerMax;

    [Header("References")]
    [SerializeField] GameObject button;
    [SerializeField] Transform onPos;
    [SerializeField] Transform offPos;

    float timer = 0;
    public bool pressed = false;

    void FixedUpdate()
    {
        if (timer > 0)
        {
            timer--;
            if (timer <= 0) Press(false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!pressed && LayerMask.LayerToName(collision.collider.gameObject.layer).Equals(GetLayer(Layer.Ball))) Press(true);
    }

    void Press(bool on)
    {
        pressed = on;

        if (on)
        {
            button.transform.DOMove(onPos.transform.position, .5f);
            if (timerMax > 0) timer = timerMax;
        }
        else button.transform.DOMove(offPos.transform.position, .5f);
    }
}