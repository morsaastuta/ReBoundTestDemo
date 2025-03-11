using UnityEngine;

public class GloveBehaviour : MonoBehaviour
{
    [Header("Commons")]
    [SerializeField] public GameObject projection;
    [SerializeField] public Transform wrist;
    [SerializeField] public Transform palm;
    [SerializeField] public AudioSource audioSource;
    [SerializeField] Animator animator;

    [Header("Handtracking exclusives")]
    [SerializeField] public OVRHand hand;

    public void Pose(int idx)
    {
        if (animator != null) animator.SetInteger("pose", idx);
    }
}
