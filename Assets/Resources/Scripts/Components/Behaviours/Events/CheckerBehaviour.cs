using UnityEngine;

public abstract class CheckerBehaviour : MonoBehaviour
{
    [SerializeField] public bool isInt = false;

    public virtual bool CheckBool()
    {
        return false;
    }

    public virtual int CheckInt()
    {
        return 0;
    }

}
