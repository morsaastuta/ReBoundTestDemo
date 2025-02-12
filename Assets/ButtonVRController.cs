using UnityEngine;

public class ButtonVRController : MonoBehaviour
{
    [SerializeField] Material material;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip pressClip;
    [SerializeField] AudioClip triggerClip;

    public void ButtonPress()
    {
        AudioManager.instance.PlaySound(pressClip, audioSource);
    }

    public void ButtonTrigger()
    {
        AudioManager.instance.PlaySound(triggerClip, audioSource);
    }
}
