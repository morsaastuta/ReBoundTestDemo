using UnityEngine;

public class DetacherComponent : MonoBehaviour
{
    void Start()
    {
        transform.parent = null;
    }
}
