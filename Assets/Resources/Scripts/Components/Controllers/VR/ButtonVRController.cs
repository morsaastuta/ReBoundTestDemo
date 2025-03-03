using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ButtonVRController : MonoBehaviour
{
    [Header("Customization")]
    [SerializeField] UnityEvent function;
    [SerializeField] Material material;
    [SerializeField] AudioClip pressClip;
    [SerializeField] AudioClip triggerClip;

    [Header("References")]
    [SerializeField] Transform root;
    [SerializeField] Transform endpoint;
    [SerializeField] MeshRenderer renderer;
    [SerializeField] AudioSource audioSource;

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
        function.Invoke();
    }

    public void ButtonUnpress()
    {
        root.DOMove(root.position, 0.5f);
    }

    public IEnumerator AutoButton()
    {
        ButtonPress();

        root.DOMove(endpoint.position, 0.25f);
        
        yield return new WaitForSeconds(0.25f);

        ButtonTrigger();
    }
}
