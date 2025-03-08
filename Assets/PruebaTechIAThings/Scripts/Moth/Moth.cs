using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Moth : MonoBehaviour
{
    public List<GameObject> _listOfSpots = new();
    public Vector3 onGoingTo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (GameObject gO in GameObject.FindGameObjectsWithTag("MothSpot"))
        {
            _listOfSpots.Add(gO);
        }
    }

    public Vector3 CheckLightsOn()
    {
        int maxLights = 0;

        Vector3 pointToGo = transform.position;

        foreach (GameObject spot in _listOfSpots)
        {
            if (spot.GetComponent<Spot>().CheckLights() > maxLights || spot.GetComponent<Spot>().CheckLights() == maxLights && onGoingTo == spot.transform.position)
            {
                maxLights = spot.GetComponent<Spot>().CheckLights();
                pointToGo = spot.transform.position;
            }
        }

        return pointToGo;
    }
}


















































//Su madre tiene una polla, que ya la quisiera yo, me dio pena por su padre el dia que se enteró, que fue en la noche de boda, quien se iba a imaginar , que iba a ser a su padre, al que lo iban a encular
