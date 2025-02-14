using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ButtonVRController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform root;
    [SerializeField] Transform endpoint;
    [SerializeField] MeshRenderer renderer;
    [SerializeField] AudioSource audioSource;

    [Header("Customization")]
    [SerializeField] Material material;
    [SerializeField] AudioClip pressClip;
    [SerializeField] AudioClip triggerClip;

    void OnEnable()
    {
        root.DOMove(root.position, 0.01f);
    }

    void Start()
    {
        if (material != null) renderer.material = material;
    }

    public void ButtonPress()
    {
        AudioManager.instance.PlaySound(pressClip, audioSource);
    }

    public void ButtonTrigger()
    {
        AudioManager.instance.PlaySound(triggerClip, audioSource);
    }

    public IEnumerator AutoButton(float s)
    {
        ButtonPress();

        root.DOMove(endpoint.position, s);
        
        yield return new WaitForSeconds(s);

        ButtonTrigger();
    }
}
