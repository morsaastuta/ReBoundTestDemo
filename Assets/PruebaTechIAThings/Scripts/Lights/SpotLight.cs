using UnityEngine;

public class SpotLight : MonoBehaviour
{
    public bool isOn;

    public bool IsOn { get => isOn; set => isOn = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //isOn = true;
    }
}
