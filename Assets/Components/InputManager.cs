using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    #region INPUT ACTION REFERENCES
    [Header("Inputs")]
    [SerializeField] public InputActionReference move;
    [SerializeField] public InputActionReference interact;
    [SerializeField] public InputActionReference shoot;
    [SerializeField] public InputActionReference switchL;
    [SerializeField] public InputActionReference switchR;
    #endregion

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

    public bool Pressed(InputActionReference input)
    {
        return input.action.triggered;
    }

    public Vector2 Inclination(InputActionReference input)
    {
        return input.action.ReadValue<Vector2>();
    }
}