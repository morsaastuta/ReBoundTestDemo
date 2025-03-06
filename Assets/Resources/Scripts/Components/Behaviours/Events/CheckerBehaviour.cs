using UnityEngine;

public abstract class CheckerBehaviour : MonoBehaviour
{
    [SerializeField] public bool isInt = false;

    public virtual bool Check()
    {
        return false;
    }

    public virtual bool Check(int rule)
    {
        return 0 >= rule;
    }

}
