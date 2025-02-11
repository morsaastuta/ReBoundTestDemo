using UnityEngine;

public class Glove : MonoBehaviour
{
    [Header("Commons")]
    [SerializeField] public GameObject projection;
    [SerializeField] public Transform wrist;
    [SerializeField] public Transform palm;
    [SerializeField] public AudioSource audioSource;

    [Header("Handtracking exclusives")]
    [SerializeField] public OVRHand hand;
}
