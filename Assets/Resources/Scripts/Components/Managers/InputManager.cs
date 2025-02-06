using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    #region INPUT ACTION REFERENCES
    [Header("Inputs")]
    [SerializeField] public InputActionReference move;
    [SerializeField] public InputActionReference look;

    [SerializeField] public InputActionReference shoot;
    [SerializeField] public InputActionReference aim;
    [SerializeField] public InputActionReference swap;
    [SerializeField] public InputActionReference pause;

    [SerializeField] public InputActionReference grabR;
    [SerializeField] public InputActionReference grabL;
    [SerializeField] public InputActionReference distanceGrabR;
    [SerializeField] public InputActionReference distanceGrabL;
    #endregion

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

    public bool Pressed(InputActionReference input)
    {
        return input.action.triggered;
    }

    public bool Holding(InputActionReference input)
    {
        return input.action.IsPressed();
    }

    public Vector2 Inclination(InputActionReference input)
    {
        return input.action.ReadValue<Vector2>();
    }
}