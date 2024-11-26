using UnityEngine;

public class OVRControllerManager : MonoBehaviour
{
    public static OVRControllerManager instance;

    public Vector3 RTouchPosition;

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        RTouchPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
    }
}
