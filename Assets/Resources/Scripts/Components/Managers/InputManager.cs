using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    #region INPUT ACTION REFERENCES

    [Header("Common inputs")]
    [SerializeField] public InputActionReference move;
    [SerializeField] public InputActionReference look;
    public InputActionReference shoot;
    public InputActionReference aim;
    public InputActionReference pause;
    public InputActionReference swap;
    public InputActionReference clear;
    [SerializeField] public InputActionReference grabR;
    [SerializeField] public InputActionReference grabL;
    [SerializeField] public InputActionReference distanceGrabR;
    [SerializeField] public InputActionReference distanceGrabL;

    [Header("Right-handed inputs")]
    [SerializeField] InputActionReference R_shoot;
    [SerializeField] InputActionReference R_aim;
    [SerializeField] InputActionReference R_pause;
    [SerializeField] InputActionReference R_swap;
    [SerializeField] InputActionReference R_clear;

    [Header("Left-handed inputs")]
    [SerializeField] InputActionReference L_shoot;
    [SerializeField] InputActionReference L_aim;
    [SerializeField] InputActionReference L_pause;
    [SerializeField] InputActionReference L_swap;
    [SerializeField] InputActionReference L_clear;

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

    public void SetHandedness(bool left)
    {
        if (!left)
        {
            shoot = R_shoot;
            aim = R_aim;
            pause = R_pause;
            swap = R_swap;
            clear = R_clear;
        }
        else
        {
            shoot = L_shoot;
            aim = L_aim;
            pause = L_pause;
            swap = L_swap;
            clear = L_clear;
        }
    }
}