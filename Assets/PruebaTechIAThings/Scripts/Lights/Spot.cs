using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour
{
    public List<SpotLight> lights = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Transform t in transform)
        {
            lights.Add(t.GetComponent<SpotLight>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int CheckLights()
    {
        int lightsOn = 0;

        foreach (SpotLight light in lights)
        {
            if (light.isOn) lightsOn++;
        }

        //Debug.Log(lightsOn);

        return lightsOn;
    }
}
