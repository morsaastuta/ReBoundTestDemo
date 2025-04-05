using Oculus.Haptics;
using UnityEngine;

public class HapticsManager : MonoBehaviour
{
    public static HapticsManager instance;

    [Header("References")]
    [SerializeField] HapticSource hapticPlayer;
    [SerializeField] public HapticClip gloveFeedback;
    [SerializeField] public HapticClip rainFeedback;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Play(HapticClip clip, Controller config)
    {
        hapticPlayer.clip = clip;
        hapticPlayer.Play(config);
    }
}