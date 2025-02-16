using DG.Tweening;
using System.Collections;
using UnityEngine;
using static Glossary;

public class ButtonVRController : MonoBehaviour
{
    [Header("Customization")]
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
    }

    public IEnumerator AutoButton(float s)
    {
        ButtonPress();

        root.DOMove(endpoint.position, s);
        
        yield return new WaitForSeconds(s);

        ButtonTrigger();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (CompareLayer(collider, Layer.Ball))
        {
            StartCoroutine(AutoButton(0.5f));
            Destroy(collider.gameObject);
        }
    }
}
